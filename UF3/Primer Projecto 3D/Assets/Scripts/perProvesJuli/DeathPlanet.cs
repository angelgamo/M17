using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlanet : MonoBehaviour
{
    public Vector3 initrialPos;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") {
            collision.transform.position = initrialPos;
        }
    }
}
