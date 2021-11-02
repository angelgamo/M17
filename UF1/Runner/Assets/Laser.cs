using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    void Start()
    {
        Invoke("destroyLaser", .1f);
    }

    private void destroyLaser()
    {
        Destroy(this.gameObject);
    }
}
