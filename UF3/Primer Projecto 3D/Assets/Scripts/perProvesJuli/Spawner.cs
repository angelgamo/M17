using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float force;
    void Start()
    {
        StartCoroutine(spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawn()
    {
        while (true)
        {
            GameObject p = AsteroidPool.SharedInstance.GetPooledObject();

            if (p != null)
            {
                p.transform.position = this.transform.position;
                p.SetActive(true);
                //p.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f)));
                p.GetComponent<Rigidbody>().AddForce(transform.up * Random.Range(-force, force));
                p.GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-force, force));
                p.GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(-force, force));
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
