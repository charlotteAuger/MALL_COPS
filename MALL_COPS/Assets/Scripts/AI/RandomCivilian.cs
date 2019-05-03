using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCivilian : MonoBehaviour
{
    [SerializeField] private GameObject[] meshes;
    private GameObject meshObject;

    private void OnEnable()
    {
        meshObject = meshes[Random.Range(0, meshes.Length)];
        meshObject.SetActive(true);
    }

    private void OnDisable()
    {
        meshObject.SetActive(false);
    }
}
