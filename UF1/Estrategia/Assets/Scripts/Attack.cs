using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private string enemyTag;

    private void Start()
    {
        enemyTag = transform.parent.GetComponent<AI>().enemyTag;

        gameObject.AddComponent<CircleCollider2D>();
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        gameObject.GetComponent<CircleCollider2D>().radius = transform.parent.GetComponent<AI>().attackRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals(enemyTag))
        {
            transform.parent.GetComponent<AI>().enemiesInAttack.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals(enemyTag))
        {
            transform.parent.GetComponent<AI>().enemiesInAttack.Remove(collision.transform);
        }
    }
}
