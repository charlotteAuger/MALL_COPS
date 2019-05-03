using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        CreateInverseCurves();
    }

    bool ended;

    [SerializeField] private List<PointOfInterest> pointsOfInterest;
    [SerializeField] private List<GameObject> exits;

    public AnimationCurve inverseUpPressureCurve;
    public AnimationCurve inverseDownPressureCurve;
    [SerializeField] AIData robberData;
    [SerializeField] AIData innocentData;

    [Header("Spawn")]
    [SerializeField] GameObject aiPrefab;
    [SerializeField] private int maxNbrOfAI;
    [HideInInspector] public List<AIController> aiInGame;
    [SerializeField] private float checkInterval;


    private void Start()
    {
        InitializeAISystem();
    }

    /// ///////////////////// MANAGEMENT
    /// 
    public void InitializeAISystem()
    {
        aiInGame.AddRange(FindObjectsOfType<AIController>());

        foreach (AIController a in aiInGame)
        {
            a.InitAI(false, innocentData);
        }

        InvokeRepeating("CheckAINbr", 0, checkInterval);
    }

    public void CheckAINbr()
    {
        if (GameManager.Instance.gameState == GameStates.END_OF_LEVEL) { return; }

        float t = GameManager.Instance.timer / GameManager.Instance.maxTimer;

        if (aiInGame.Count < maxNbrOfAI)
        {
            SpawnAI();
        }

        int n = (t < 0.5f) ? 3 : ((t < 0.8f) ? 2 : 1);
        int r = 0;

        foreach (AIController a in aiInGame)
        {
            if (a.isRobber)
            {
                r++;
            }
        }

        if (r < n)
        {
            CorruptAI();
        }
    }

    private void Update()
    {
        if (!ended && GameManager.Instance.gameState == GameStates.END_OF_LEVEL)
        {
            ended = true;
            InnocentAll();
        }
    }

    public void SpawnAI()
    {
        int r = Random.Range(0, exits.Count);
        Vector3 spawnPoint = exits[r].transform.position;

        AIController aiController = Instantiate(aiPrefab, spawnPoint, Quaternion.identity).GetComponent<AIController>();
        aiController.InitAI(false, innocentData);
    }

    public void InnocentAll()
    {
        foreach (AIController a in aiInGame)
        {
            a.isRobber = false;
        }
    }

    public void CorruptAI()
    {
        bool innocentFound = false;
        AIController targetAI = null;
        int i = 0;
        while (!innocentFound && i < aiInGame.Count)
        {
            AIController checkedAI = aiInGame[i];
            if (!checkedAI.isRobber)
            {
                targetAI = checkedAI;
                innocentFound = true;
            }
            i++;
        }

        if (targetAI != null)
        {
            targetAI.isRobber = true;
            targetAI.stats = robberData;
        }
    }

 
    /// ///////////////////// SERVICES

    public Vector3 GetSafestExit(Vector3 position)
    {
        int r = Random.Range(0, exits.Count);

        return exits[r].transform.position;
    }

    public PointOfInterest GetAnIP(Vector3 position)
    {
        List<PointOfInterest> temps = new List<PointOfInterest>();
        temps.AddRange(pointsOfInterest);

        float dist = Mathf.Infinity;
        PointOfInterest closest = null;

        foreach (PointOfInterest p in temps)
        {
            float d = (position - p.transform.position).magnitude;
            if (d < dist)
            {
                closest = p;
                dist = d;
            }
        }

        temps.Remove(closest);

        int r = Random.Range(0, temps.Count);

        return temps[r];
    }

    public Vector3 GetClosestPlayerPosition(Vector3 position)
    {
        Vector3 firstPlayerPosition = GameManager.Instance.players[0].position;
        Vector3 secondPlayerPosition = GameManager.Instance.players[1].position;

        float d1 = Vector3.Distance(position, firstPlayerPosition);
        float d2 = Vector3.Distance(position, secondPlayerPosition);

        return d1 < d2 ? firstPlayerPosition : secondPlayerPosition;
    }

    private void CreateInverseCurves()
    {
        inverseUpPressureCurve = new AnimationCurve();
        for (int i = 0; i < robberData.pressureUpCurve.length; i++)
        {
            Keyframe inverseKey = new Keyframe(robberData.pressureUpCurve.keys[i].value, robberData.pressureUpCurve.keys[i].time);
            inverseUpPressureCurve.AddKey(inverseKey);
        }

        inverseDownPressureCurve = new AnimationCurve();
        for (int i = 0; i < robberData.pressureDownCurve.length; i++)
        {
            Keyframe inverseKey = new Keyframe(robberData.pressureDownCurve.keys[i].value, robberData.pressureDownCurve.keys[i].time);
            inverseDownPressureCurve.AddKey(inverseKey);
        }

    }
}
