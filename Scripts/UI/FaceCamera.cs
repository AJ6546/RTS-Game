using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        // So that the object attached to is always facing camera
        transform.forward = Camera.main.transform.forward;
    }
}
