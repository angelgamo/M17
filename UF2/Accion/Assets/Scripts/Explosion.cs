using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine("die");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player") {
            collision.GetComponent<MovementController>().enabled = false;
        }
    }
    IEnumerator die() {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
