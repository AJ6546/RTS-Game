using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] float time;
    public void DestroyGameObject()
    {
        Destroy(gameObject, time);
    }
}
