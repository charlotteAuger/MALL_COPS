using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watchable : MonoBehaviour
{
    public delegate void OnWatched(bool _condition);
    public event OnWatched EventOnWatched;
    [HideInInspector]public List<FieldOfView> peopleWatching = new List<FieldOfView>();
    
    public void CallWatchedEvent(bool _watched)
    {
        EventOnWatched?.Invoke(_watched);
        //Debug.Log(_watched ? "Watched!" : "Not watched");
    }
}
