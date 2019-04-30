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

    private NavMeshAgent navMeshAgent;
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
            currentState.OnStateExit(this);
            currentState = newState;
            currentState.OnStateEnter(this);
        }

        if (currentState.GetType() != typeof(CaughtState) && actorsAround.Count >= 0 && avoidance)
        {
            AvoidActors(actorsAround.ToArray());
        }
    }

    public void MoveTowards(Vector3 targetPosition, float speed)
    {
        rB.velocity = (targetPosition - transform.position).normalized * speed;
    }

    public void LookTowards(Vector3 targetPosition)
    {

    }

    public void AvoidActors(GameObject[] actors)
    {
        Vector3 averagedDirection = Vector3.zero;

        for (int i = 0; i < actors.Length; i++)
        {
            averagedDirection += actors[i].transform.position - transform.position;
        }

        averagedDirection = averagedDirection / actors.Length;
        averagedDirection = new Vector3(averagedDirection.x, 0, averagedDirection.z).normalized;

        rB.AddForce(-averagedDirection * stats.avoidanceSpeed, ForceMode.Impulse);
        //add velocity
    }

    public void GetPath(Vector3[] results)
    {

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
            actorsAround.Remove(other.gameObject);
        }
    }


}
