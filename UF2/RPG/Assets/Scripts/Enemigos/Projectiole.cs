using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiole : MonoBehaviour
{
    // Start is called before the first frame update
    float damaga;
    [HideInInspector]
    public CharacterCombat cc;
    [HideInInspector]
    public string tagToIgnore;

    private void Awake()
    {
        StartCoroutine("die");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterCombat>() && collision.transform.tag != tagToIgnore)
        {

            cc.Attack(collision.GetComponent<CharacterStats>());
            //Destroy(this.gameObject);
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Other")
        {
            Destroy(this.gameObject);
        }
        
    }
    IEnumerator die() {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
