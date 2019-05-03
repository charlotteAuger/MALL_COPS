using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCivilian : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Animator[] animators;
    [HideInInspector] public Renderer rend;
    [HideInInspector] public Animator anim;

    private void OnEnable()
    {
        int r = Random.Range(0, renderers.Length);
        rend = renderers[r];
        anim = animators[r];
        rend.transform.parent.gameObject.SetActive(true);

    }

    private void OnDisable()
    {
        rend.transform.parent.gameObject.SetActive(false);
    }
}
