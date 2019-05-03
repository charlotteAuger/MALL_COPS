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
        Invoke("Poof", 1f);
    }

    private void Poof()
    {
        Destroy(this.gameObject);
    }
}
