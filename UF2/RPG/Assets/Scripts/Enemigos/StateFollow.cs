using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateFollow : State
{
    EnemyIA ia;
    float maxDistance;
    float distance;
    float margin;
    float playerDistance;
    Rigidbody2D rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    public override void exit()
    {
       
    }

    public override void init()
    {
        ia = gameObject.GetComponent<EnemyIA>();
        maxDistance = ia.visionRange;
        distance = ia.attackDistance;
        margin = ia.attackMargin / 3;
    }

    public override void update()
    {
        /*if (isRanged)
        {
            if (Vector2.Distance(transform.position, ia.playerPos.position) <= ia.visionRange && Vector2.Distance(transform.position, ia.playerPos.position) > distance && Vector2.Distance(transform.position, ia.playerPos.position) > minDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, ia.playerPos.transform.position, ia.speed * Time.deltaTime);
            }
            else 
            {
                transform.position = Vector2.MoveTowards(transform.position, ia.playerPos.transform.position, ia.speed/2 * -1 * Time.deltaTime);
            }
        }
        else 
        {
            if (Vector2.Distance(transform.position, ia.playerPos.position) <= ia.visionRange && Vector2.Distance(transform.position, ia.playerPos.position) > distance)
            {
                transform.position = Vector2.MoveTowards(transform.position, ia.playerPos.transform.position, ia.speed * Time.deltaTime);
            }
        }*/

        playerDistance = Vector2.Distance(transform.position, ia.playerPos.position);

        if (playerDistance > maxDistance) // patrulla
            ia.changeState(ia.states[0]);
        else if (playerDistance < distance - margin) // se acerca
            //transform.position = Vector2.MoveTowards(transform.position, ia.playerPos.transform.position, ia.speed / 2 * -1 * Time.deltaTime);
            rb.MovePosition(Vector2.MoveTowards(transform.position, ia.playerPos.transform.position, ia.speed * -1 * Time.deltaTime));
        else if (playerDistance > distance + margin) // se aleja
            //transform.position = Vector2.MoveTowards(transform.position, ia.playerPos.transform.position, ia.speed * Time.deltaTime);
            rb.MovePosition(Vector2.MoveTowards(transform.position, ia.playerPos.transform.position, ia.speed * Time.deltaTime));
        else // ataca
            ia.changeState(ia.states[2]);

        if (Vector2.Distance(transform.position, ia.playerPos.position) > ia.visionRange)
        {
            ia.changeState(ia.states[0]);
        } 
    }
}
