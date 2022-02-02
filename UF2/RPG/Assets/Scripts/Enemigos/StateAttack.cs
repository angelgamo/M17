using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : State
{
    EnemyIA ia;
    float distance;
    float margin;
    float playerDistance;

    public override void exit()
    {
        //this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void init()
    {
        //this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        ia = gameObject.GetComponent<EnemyIA>();
        distance = ia.attackDistance;
        margin = ia.attackMargin;
    }

    public override void update()
    {
        playerDistance = Vector2.Distance(transform.position, ia.playerPos.position);

        if (playerDistance < distance - margin && !ia.isMelee)
            ia.changeState(ia.states[0]);
        else if (playerDistance > distance + margin)
            ia.changeState(ia.states[0]);
        else
            transform.GetChild(0).GetComponent<Weapon3>().Attack();
    }



}
