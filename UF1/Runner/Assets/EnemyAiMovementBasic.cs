using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiMovementBasic : MonoBehaviour
{
    public float speed = -.6f;
    void Start()
    {
        GetComponent<Rigidbody2D>().drag = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector3(speed, 0, 0);
    }
}
