using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int energy;

    void Start()
    {
        Destroy(this.gameObject, 5f);
    }
}
