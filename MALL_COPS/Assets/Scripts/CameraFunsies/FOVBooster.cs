using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVBooster : MonoBehaviour
{
    public Camera cam;
    float lerp = 0.1f;
    internal float originFOV;
    float currentFOV;
    public float resetLerp;

    private void Start()
    {
        originFOV = cam.fieldOfView;
        currentFOV = originFOV;
    }

    private void FixedUpdate()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentFOV, lerp);
    }

    public void SetFOV(float _newFOV, float _lerp)
    {
        currentFOV = _newFOV;
        lerp = _lerp;
    }

    public void ResetFOV()
    {
        currentFOV = originFOV;
        lerp = resetLerp;
    }
}
