using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemic : MonoBehaviour
{
    AI ai;
    public float speed;
    bool go;

    void Start()
    {
        ai  = this.GetComponent<AI>();
        StartCoroutine(CheckGroup());
        StartCoroutine(GoAnyway());
    }
    
    void Update()
    {
        if (ai.ally || ai.attack || ai.target)
        {
            attack();
        }
        else if (go)
        {
            noAttack();
        }
        else
        {
            nothing();
        }
    }

    IEnumerator CheckGroup()
    {
        while (true)
        {
            if (ai.alliesInVision.Count >= 4)
            {
                go = true;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator GoAnyway()
    {
        yield return new WaitForSeconds(10f);
        go = true;
    }

    private void noAttack()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed * Time.deltaTime);
        this.gameObject.GetComponent<AI>().speed = 0;
    }

    private void attack()
    {
        this.gameObject.GetComponent<AI>().speed = 1;
    }

    private void nothing()
    {
        this.gameObject.GetComponent<AI>().speed = 0;
    }
}
