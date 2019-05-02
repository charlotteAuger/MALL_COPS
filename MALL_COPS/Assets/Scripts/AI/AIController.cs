﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public State currentState;
    public AIData stats;
    public string state;


    public bool avoidance;
    private List<GameObject> actorsAround = new List<GameObject>();

    public bool isRobber;
    private float pressure;
    public bool willRob => (pressure <= 0 && isRobber);
    public bool watched;
    public float timeWatched;

    [Header("References")]
    [SerializeField] private Transform stolenObjectAnchor;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody rB;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private Renderer[] renders;
    [SerializeField] public Watchable watchable;




    private void Start()
    {
        InitAI(isRobber, stats);
    }

    public void InitAI(bool _isRobber, AIData _stats)
    {
        //Initialize if robber or civilian
        isRobber = _isRobber;
        stats = _stats;

        //Set default begining state
        currentState = new GoingToIPState();
        currentState.OnStateEnter(this);


        //Subscribe to watchable
        watchable.EventOnWatched += Watched;

        //Sensor update function
        InvokeRepeating("UpdateActorsAround", 0, 0.05f);
    }

    private void OnDestroy()
    {
        watchable.EventOnWatched -= Watched;
    }

    private void FixedUpdate()
    {
        if (isRobber)
        {
            if (watched && timeWatched < stats.pressureUpTime)
            {
                timeWatched += Time.deltaTime*watchable.peopleWatching.Count;
                pressure = stats.pressureUpCurve.Evaluate(timeWatched / stats.pressureUpTime);
            }
            else if (!watched && timeWatched > 0)
            {
                timeWatched -= Time.deltaTime;
                pressure = stats.pressureDownCurve.Evaluate(timeWatched / stats.pressureDownTime);
            }
        }

        //State actions
        State newState = currentState.StateEffect(this, Time.fixedDeltaTime);

        //If the state transitiones to another one, store it
        if (newState != null)
        {
            state = newState.GetType().ToString();
            currentState.OnStateExit(this);
            currentState = newState;
            currentState.OnStateEnter(this);
        }

        if (currentState.GetType() != typeof(CaughtState) && currentState.GetType() != typeof(TackledState) && currentState.GetType() != typeof(InShopState) && actorsAround.Count > 0 && avoidance)
        {
            AvoidActors(actorsAround);
        }
        Debug.DrawRay(transform.position, rB.velocity, Color.magenta);

        if (rB.velocity.magnitude > 0.5f)
        {
            LookTowards(transform.position + rB.velocity);
        }

    }

    public void SetPresence(bool presence)
    {
        foreach (Collider c in colliders)
        {
            c.enabled = presence;
        }

        foreach (Renderer r in renders)
        {
            r.enabled = presence;
        }

        rB.isKinematic = !presence;
    }

    public void SetCollider(bool state)
    {
        foreach (Collider c in colliders)
        {
            c.enabled = state;
        }
    }


    /// ////////////////////////////////////////// MOVEMENT FUNCTIONS
    ///
    public void MoveTowards(Vector3 targetPosition, float speed)
    {
        Vector3 v = (targetPosition - transform.position).normalized;
        rB.velocity = v.normalized * speed;
        Debug.DrawRay(transform.position, v, Color.blue);
    }

    public void LookTowards(Vector3 targetPosition)
    {
        Vector3 lookDirection = targetPosition - transform.position;
        lookDirection = new Vector3(lookDirection.x, 0, lookDirection.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection, Vector3.up), stats.rotationSpeed);
    }

    public void StopMovement()
    {
        rB.velocity = Vector3.zero;
    }

    public Vector3 AvoidActors(List<GameObject> actors)
    {
        Vector3 averagedDirection = Vector3.zero;

        int n = actors.Count;
        for (int i = 0; i < actors.Count; i++)
        {
            Vector3 dir = actors[i].transform.position - transform.position;
            averagedDirection += dir;

        }

        averagedDirection = averagedDirection / n;

        float m = averagedDirection.magnitude;
        averagedDirection = new Vector3(averagedDirection.z, 0, -averagedDirection.x);//.normalized * (3f - m);

        rB.velocity -= averagedDirection * stats.avoidanceSpeed * currentState.speed;
        Debug.DrawRay(transform.position, -averagedDirection, Color.green);

        rB.velocity = Vector3.ClampMagnitude(rB.velocity, currentState.speed);
        return averagedDirection * stats.avoidanceSpeed;
    }

    public Vector3[] GetPath(Vector3 targetPosition)
    {
        navMeshAgent.enabled = true;
        NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.CalculatePath(targetPosition, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            Vector3[] c = path.corners;
            navMeshAgent.enabled = false;
            return c;
        }
        else
        {
            return null;
        }
    }


    /// ////////////////////////////////////////// DETECTION
    /// 

    private void UpdateActorsAround()
    {
        Collider[] shortColliders = Physics.OverlapSphere(transform.position, 1.5f);
        Collider[] longColliders = Physics.OverlapSphere(transform.position, 4f);
        actorsAround.Clear();

        foreach (Collider c in shortColliders)
        {
            if (!actorsAround.Contains(c.gameObject) && c.gameObject != this.gameObject && (c.gameObject.tag == "Civilian" || c.gameObject.tag == "Player"))
            {
                actorsAround.Add(c.gameObject);
            }
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Civilian" || other.tag == "Player") // or player
        {
            print(gameObject.name + "  " + other.gameObject.name);
            actorsAround.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Civilian" || other.tag == "Player") // or player
        {
            print(gameObject.name + "  " + other.gameObject.name + " OUT");
            actorsAround.Remove(other.gameObject);
        }
    }*/

    public void OnTackled()
    {
        State s = currentState.OnTackled(this);
        if (s != null)
        {
            state = s.GetType().ToString();
            currentState.OnStateExit(this);
            currentState = s;
            currentState.OnStateEnter(this);
        }
    }

    public void OnSeeTackle(Vector3 tacklePosition)
    {
        State s = currentState.OnSeeTackle(this, tacklePosition);
        if (s != null)
        {
            state = s.GetType().ToString();
            currentState.OnStateExit(this);
            currentState = s;
            currentState.OnStateEnter(this);
        }
    }       

    public void Watched(bool state)
    {
        watched = state;
        timeWatched = state ? AIManager.instance.inverseUpPressureCurve.Evaluate(pressure)*stats.pressureUpTime : AIManager.instance.inverseDownPressureCurve.Evaluate(pressure)*stats.pressureDownTime;
    }

    /// ////////////////////////////////////////// STEALING
    /// 
    public GameObject SetupStolenItem(PointOfInterest ip)
    {
        GameObject item = Instantiate(ip.valuableObject, stolenObjectAnchor, false);
        item.transform.localPosition = Vector3.zero;
        return item;
    }

    public void DropStolenItem(GameObject item)
    {
        item.transform.parent = null;
        Rigidbody rB = item.GetComponent<Rigidbody>();
        rB.isKinematic = false;
    }

    /// ////////////////////////////////////////// DEBUG
    /// 
    public void PPrint(string stuff)
    {
        print(stuff);
    }
}
