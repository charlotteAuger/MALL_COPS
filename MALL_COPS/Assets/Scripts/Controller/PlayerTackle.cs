using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTackle : MonoBehaviour
{
    public PlayerMovement movement;
    public float chargeSpeed;

    private void Start()
    {
        InputManager.Instance.TacklePressed_1 += Tackle;
    }

    void Tackle()
    {

    }
}
