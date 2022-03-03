using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickOnPlatorm : MonoBehaviour
{

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "platform")
        {
            transform.parent = collision.transform;

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "platform")
        {
            transform.parent = null;
        }
    }
}
