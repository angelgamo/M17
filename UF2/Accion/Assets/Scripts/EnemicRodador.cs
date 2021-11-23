using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemicRodador : MonoBehaviour
{
    // Start is called before the first frame update
    public float spd;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(0, 0, spd);
        GetComponent<Rigidbody2D>().AddTorque(spd);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "boss") {
            Destroy(this.gameObject);
        }
    }
}
