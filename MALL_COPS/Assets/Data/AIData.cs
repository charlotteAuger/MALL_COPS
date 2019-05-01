using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIAnimations { };

[CreateAssetMenu(fileName = "AIData", menuName = "AIData", order = 0)]
public class AIData : ScriptableObject
{
    [Header("Looking time")]
    public float tackleLookingTime_min;
    public float tackleLookingTime_max;
    public float ipLookingTime_min;
    public float ipLookingTime_max;
    public float shopLookingDistance;

    [Header("Movement")]
    public float walkSpeed_min;
    public float walkSpeed_max;
    public float fleeingSpeed;
    public float avoidanceSpeed;
    public float stopDistance;
    public float rotationSpeed;
}
