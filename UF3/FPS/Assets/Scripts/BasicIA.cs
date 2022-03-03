using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicIA : MonoBehaviour
{
    public Transform target;
    bool following = false;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            GetComponent<NavMeshAgent>().destination = target.position;
            transform.LookAt(target.transform.position);
            
        }
    }

    public void activarFollow(Transform target)
    {
        this.following = true;
    }

    public void perderVista()
    {
        this.following = false;
    }
}
