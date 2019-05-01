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

        //return temps[r];
        return pointsOfInterest[0];
    }
}
