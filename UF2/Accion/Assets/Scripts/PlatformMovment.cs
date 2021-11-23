using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovment : MonoBehaviour
{
    
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "boss") {
            this.gameObject.SetActive(false);
        }
    }
}
