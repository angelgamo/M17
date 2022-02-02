using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject pro;
    [HideInInspector]
    public Vector3 offset;
    EnemyIA ia;
    CharacterCombat cc;
    public float Speed;
    public Transform childTransform;

    void Start()
    {
        ia = gameObject.GetComponentInParent(typeof(EnemyIA)) as EnemyIA;
        cc = gameObject.GetComponentInParent(typeof(CharacterCombat)) as CharacterCombat;
    }

    void shoot() {
        if (pro == null)
            return;

        GameObject newBala = Instantiate(pro, childTransform.position , childTransform.rotation);
        newBala.GetComponent<Rigidbody2D>().velocity = newBala.transform.right * Speed;
        newBala.GetComponent<Projectiole>().cc = cc;
        newBala.GetComponent<Projectiole>().tagToIgnore = this.transform.root.tag;
    }

    float GetAngle(Vector2 point1, Vector2 point2)
    {
        Vector2 delta = point1 - point2;
        return ((Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg) + 180);
    }
}
