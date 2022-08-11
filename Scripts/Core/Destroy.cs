using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] float time = 5f;
    [SerializeField] GameObject targetToDestroy = null;
    void Start()
    {
        if (targetToDestroy != null)
            Destroy(targetToDestroy, time);
        else
            Destroy(gameObject, time);
    }
}
