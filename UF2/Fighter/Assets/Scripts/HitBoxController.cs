using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    [Header("Hitbox Variables")]
    [SerializeField] PlayerSO playerSO;
    HealthManager healthManager;

    private void Start()
    {
        healthManager = transform.parent.GetComponent<HealthManager>();
    }

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
        if (!healthManager.isDead)
        {
            foreach (int layer in playerSO.enemyLayer)
            {
                if (collision.gameObject.layer == layer)
                {
                    collision.transform.parent.GetComponent<HealthManager>().RecieveDamage(playerSO.damage);
                }
            }
        }
    }
}
