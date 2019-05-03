using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private List<PointOfInterest> pointsOfInterest;
    [SerializeField] private List<GameObject> exits;

    public AnimationCurve inverseUpPressureCurve;
    public AnimationCurve inverseDownPressureCurve;
    [SerializeField] AIData robberData;


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

    public Vector3 GetSafestExit(Vector3 position)
    {
        int r = Random.Range(0, exits.Count);

        return exits[r].transform.position;
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
