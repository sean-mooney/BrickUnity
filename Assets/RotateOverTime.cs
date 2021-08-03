using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.right;
    public float rotationSpeed = 2;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotationAxis * rotationSpeed);
    }
}
