using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraqnadeThrow : MonoBehaviour
{
    bool launched;
    [SerializeField]
    GameObject granade;
    GameObject ActiveGranade;
    [SerializeField]
    Transform Hand;
    [SerializeField]
    float force;
    void Start()
    {
        launched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !launched)
        {
            launched = true;
            GameObject gran =  Instantiate(granade, Hand.position, Quaternion.identity);
            gran.transform.parent = transform;
            gran.GetComponent<Rigidbody>().useGravity = false;
            gran.GetComponent<Granade>().startEx(7);
            ActiveGranade = gran;
        }
        if (Input.GetKeyUp(KeyCode.Q) && launched)
        {
            try
            {
                ActiveGranade.transform.parent = null;
                ActiveGranade.GetComponent<Rigidbody>().useGravity = true;
                ActiveGranade.GetComponent<Rigidbody>().AddForce(transform.up * force + transform.forward * force);
            }
            catch (System.Exception)
            {
            }
            StartCoroutine(recharge());
        }

    }


    IEnumerator recharge() 
    {
        yield return new WaitForSeconds(10);
        launched = false;
    }
}
