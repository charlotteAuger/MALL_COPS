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
    }

    [SerializeField] private List<PointOfInterest> pointsOfInterest;
    [SerializeField] private List<GameObject> exits;

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
        return Vector3.zero;
    }
}
