using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //this.GetComponent<Rigidbody>().velocity = this.transform.forward * Time.deltaTime * 400;
            this.GetComponent<Rigidbody>().AddForce(transform.forward*2);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //this.GetComponent<Rigidbody>().velocity = this.transform.forward * Time.deltaTime * -400;
            this.GetComponent<Rigidbody>().AddForce(transform.forward * -2);
        }
        else 
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().AddForce(transform.up * 200);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.rotation *= Quaternion.Euler(0, 1, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.rotation *= Quaternion.Euler(0, -1, 0);
        }
        else {
            this.transform.rotation *= Quaternion.Euler(0, 0, 0);
        }
    }
}
