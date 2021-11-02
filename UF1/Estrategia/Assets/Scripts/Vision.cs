using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    private string allyTag;
    private string enemyTag;

    private void Start()
    {
        allyTag = transform.parent.tag;
        enemyTag = transform.parent.GetComponent<AI>().enemyTag;

        gameObject.AddComponent<CircleCollider2D>();
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        gameObject.GetComponent<CircleCollider2D>().radius = transform.parent.GetComponent<AI>().visionRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals(enemyTag))
        {
            transform.parent.GetComponent<AI>().enemiesInVision.Add(collision.transform);
        }
        if (collision.transform.tag.Equals(allyTag))
        {
            transform.parent.GetComponent<AI>().alliesInVision.Add(collision.GetComponent<AI>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals(enemyTag))
        {
            transform.parent.GetComponent<AI>().enemiesInVision.Remove(collision.transform);
        }
        if (collision.transform.tag.Equals(allyTag))
        {
            transform.parent.GetComponent<AI>().alliesInVision.Remove(collision.GetComponent<AI>());
        }
    }
}
