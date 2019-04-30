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

    [SerializeField] private List<GameObject> pointsOfInterst;

    public Vector3 GetAnIP(Vector3 position)
    {
        List<GameObject> temps = new List<GameObject>();
        temps.AddRange(pointsOfInterst);

        float dist = Mathf.Infinity;
        GameObject closest = null;

        foreach (GameObject p in temps)
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

        return temps[r].transform.position;
    }
}
