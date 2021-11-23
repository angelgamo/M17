using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Vector3 centerOfMass;

    private void Start()
    {
        GetComponent<Rigidbody2D>().centerOfMass = centerOfMass;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ArrowController>() == null)
        {
            print("Flecha " + collision.name);
            /*
            if (collision.tag == "Player" || collision.transform.parent.tag == "Player" || collision.transform.parent.transform.parent.tag == "Player")
            { 
                //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                //transform.parent = collision.transform;
            }else
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }*/
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass, 0.1f);
    }
}
