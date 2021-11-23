using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTest : MonoBehaviour
{
    public GameObject arrow;
    public Vector3 spawnPos;
    public Vector2 velocity;

    private void Update()
    {
        if (Input.GetKeyDown("n")) Shoot();
    }

    void Shoot()
    {
        GameObject clone = Instantiate(arrow, spawnPos, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
        Destroy(clone, 2f);
    }
}
