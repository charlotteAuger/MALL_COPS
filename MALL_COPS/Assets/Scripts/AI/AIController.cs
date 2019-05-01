using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public State currentState;
    public bool isRobber;
    public AIData stats;
    public bool avoidance;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody rB;
    private List<GameObject> actorsAround = new List<GameObject>();

    private void Start()
    {
        InitAI(true, stats);
    }

    public void InitAI(bool _isRobber, AIData _stats)
    {
        //Initialize if robber or civilian
        isRobber = _isRobber;
        stats = _stats;

        //Set default begining state
        currentState = new GoingToIPState();
        currentState.OnStateEnter(this);
    }

    private void FixedUpdate()
    {
        //State actions
        State newState = currentState.StateEffect(this,Time.fixedDeltaTime);

        //If the state transitiones to another one, store it
        if (newState != null)
        {
            print(newState.GetType().ToString());
            currentState.OnStateExit(this);
            currentState = newState;
            currentState.OnStateEnter(this);
        }

        if (currentState.GetType() != typeof(CaughtState) && currentState.GetType() != typeof(TackledState) && actorsAround.Count > 0 && avoidance)
        {
            print("avoiding");
            AvoidActors(actorsAround.ToArray());
        }

    }

    public void MoveTowards(Vector3 targetPosition, float speed)
    {
        Vector3 v = (targetPosition - transform.position).normalized;
        rB.velocity = v.normalized * speed;
        LookTowards(targetPosition);
    }
    
    public void LookTowards(Vector3 targetPosition)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position, Vector3.up), stats.rotationSpeed);
    }

    public void StopMovement()
    {
        rB.velocity = Vector3.zero;
    }

    public Vector3 AvoidActors(GameObject[] actors)
    {
        Vector3 averagedDirection = Vector3.zero;

        for (int i = 0; i < actors.Length; i++)
        {
            averagedDirection += actors[i].transform.position - transform.position;
        }

        averagedDirection = averagedDirection / actors.Length;
        averagedDirection = new Vector3(averagedDirection.x, 0, averagedDirection.z).normalized;

        rB.velocity -= averagedDirection * stats.avoidanceSpeed;
        rB.velocity = Vector3.ClampMagnitude(rB.velocity, currentState.speed);
        return averagedDirection * stats.avoidanceSpeed;
    }

    public Vector3[] GetPath(Vector3 targetPosition)
    {
        navMeshAgent.enabled = true;
        NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.CalculatePath(targetPosition, path))
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

    public void PPrint(string stuff)
    {
        print(stuff);
    }

    private void OnTriggerEnter(Collider other)
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
    }


}
