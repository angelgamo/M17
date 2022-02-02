using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleHit : MonoBehaviour
{
    [HideInInspector]
    public CharacterCombat cc;
    [HideInInspector]
    public string tagToIgnore;

    void Start()
    {
        cc = gameObject.GetComponentInParent(typeof(CharacterCombat)) as CharacterCombat;
        tagToIgnore = this.transform.root.tag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterCombat>() && collision.transform.tag != tagToIgnore)
        {
            cc.Attack(collision.GetComponent<CharacterStats>());
        }
    }
    IEnumerator die()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
