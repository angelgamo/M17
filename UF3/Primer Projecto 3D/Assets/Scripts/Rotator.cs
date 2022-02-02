using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 axis;
    [Range(0f,1000f)]
    public float speed;
    public bool inverseDirection;

    void Update()
    {
        transform.Rotate(axis, inverseDirection ? -speed : speed * Time.deltaTime);    
    }
}
