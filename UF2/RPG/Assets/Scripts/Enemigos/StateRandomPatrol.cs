using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRandomPatrol : State
{
    EnemyIA ia;
    Vector3 destination;
    Vector3 initialPos;
    public float radi;
    Rigidbody2D rb;
    private void Start()
    {
        initialPos = this.transform.position;
        rb = this.GetComponent<Rigidbody2D>();
    }
    public override void exit()
    {

    }

    public override void init()
    {
        ia = gameObject.GetComponent<EnemyIA>();
    }

    public override void update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, destination, ia.speed * Time.deltaTime);
        rb.MovePosition(Vector2.MoveTowards(transform.position, destination, ia.speed * Time.deltaTime));
        if (Vector2.Distance(transform.position, destination) < 0.1f) 
        {
            destination = createDestination();
        }
        if (Vector2.Distance(transform.position, ia.playerPos.position) < 5f)
        {
            ia.changeState(ia.states[1]);
        }
    }

    Vector3 createDestination() 
    {
        return new Vector3(Random.Range(-radi, radi),Random.Range(-radi, radi),0)+initialPos;
    }
}
