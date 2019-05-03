using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Rigidbody rB;
    [SerializeField] private Collider c;

    public void Drop()
    {
        rB.isKinematic = false;
        c.enabled = true;
        ///forward force
    }
}
