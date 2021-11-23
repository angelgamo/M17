using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goomba : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(-3, this.GetComponent<Rigidbody2D>().velocity.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(-3, this.GetComponent<Rigidbody2D>().velocity.y, 0);
    }
}
