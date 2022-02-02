using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon3 : MonoBehaviour
{
    Transform enemyTransform;
    public EnemyIA ia;
    float angle;

    private void Start()
    {
        enemyTransform = transform.parent;
        ia = transform.root.gameObject.GetComponent<EnemyIA>();
    }

    private void Update()
    {
        angle = GetAngle(enemyTransform.position, ia.playerPos.position);

        WeaponRotation();

        
    }

    public void Attack()
    {
        /*if (Vector2.Distance(transform.position, ia.playerPos.position) < ia.range)
        {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
        }*/
        //Debug.Log("hola");
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
    }

    void WeaponRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    float GetAngle(Vector2 point1, Vector2 point2)
    {
        Vector2 delta = point1 - point2;
        return ((Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg) + 180);
    }
}
