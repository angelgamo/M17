using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    [Header("Hitbox Variables")]
    [SerializeField] Damage damageSO;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHit(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnHit(collision);
    }



    void OnHit(Collider2D collision)
    {
        if (collision.gameObject.layer == damageSO.enemyLayer && collision.transform.parent.GetComponent<HealthGetter>() != null)
        {
            if (collision.transform.parent.GetComponent<HealthGetter>().hpManager.onShield)
            {
                float angle = Angle(transform.position, collision.transform.position);
                if (collision.transform.parent.GetComponent<HealthGetter>().hpManager.flip)
                {
                    angle += 180;
                    angle %= 360;
                }
                if (90 < angle && angle < 270)
                {
                    return;
                }
            }
            collision.transform.parent.GetComponent<HealthGetter>().hpManager.RecieveDamage2(damageSO.damage);
        }
        else if (collision.gameObject.layer == damageSO.enemyLayer && collision.GetComponent<HealthGetter>() != null)
        {
            collision.GetComponent<HealthGetter>().hpManager.RecieveDamage2(damageSO.damage);
        }
    }

    float Angle(Vector2 point1, Vector2 point2)
    {
        Vector3 delta = point1 - point2;
        return (Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg) + 180;
    }
}
