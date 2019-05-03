using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCivilian : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [HideInInspector] public Renderer rend;

    private void OnEnable()
    {
        rend = renderers[Random.Range(0, renderers.Length)];
        rend.transform.parent.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        rend.transform.parent.gameObject.SetActive(false);
    }
}
