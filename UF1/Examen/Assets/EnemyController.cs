using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public delegate void EnemyDeath(string color);
    public event EnemyDeath onEnemyDeath;

    public int healtPoints = 3;

    public Sprite black;
    public bool isWhite = true;

    public GameObject bullet;
    public Vector3 bulletOffset;
    public float bulletVelocity;

    public Sprite blackBullet;

    void Start()
    {
        transform.Rotate(new Vector3(0, 0, 180f));
        if (!isWhite)
        {
            GetComponent<SpriteRenderer>().sprite = black;
        }

        StartCoroutine(Shoot());

        this.tag = isWhite ? "white" : "black";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (collision.tag == "special")
            {
                for (int i = 0; i < collision.GetComponent<BulletController>().energy; i++)
                {
                    GetHit();
                }
            }
            else
            {
                if (collision.tag == tag)
                {
                    // Nothing
                }
                else
                {
                    GetHit();
                }

                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            GameObject bulletClone = Instantiate(bullet, transform.position + bulletOffset, transform.rotation);
            bulletClone.tag = isWhite ? "white" : "black";
            bulletClone.layer = 7;
            bulletClone.GetComponent<Rigidbody2D>().velocity = transform.up * bulletVelocity;
            if (!isWhite) bulletClone.GetComponent<SpriteRenderer>().sprite = blackBullet;

            GameObject bulletClone1 = Instantiate(bullet, transform.position + bulletOffset, transform.rotation);
            bulletClone1.tag = isWhite ? "white" : "black";
            bulletClone1.layer = 7;
            bulletClone1.transform.Rotate(new Vector3(0, 0, 20f));
            bulletClone1.GetComponent<Rigidbody2D>().velocity = bulletClone1.transform.up * bulletVelocity;
            if (!isWhite) bulletClone1.GetComponent<SpriteRenderer>().sprite = blackBullet;
            
            GameObject bulletClone2 = Instantiate(bullet, transform.position + bulletOffset, transform.rotation);
            bulletClone2.tag = isWhite ? "white" : "black";
            bulletClone2.layer = 7;
            bulletClone2.transform.Rotate(new Vector3(0, 0, -20f));
            if (!isWhite) bulletClone2.GetComponent<SpriteRenderer>().sprite = blackBullet;
            bulletClone2.GetComponent<Rigidbody2D>().velocity = bulletClone2.transform.up * bulletVelocity;

            yield return new WaitForSeconds(1f);
        }
    }

    void GetHit()
    {
        healtPoints--;
        if (healtPoints <= 0)
        {
            if (onEnemyDeath != null) onEnemyDeath.Invoke(this.tag);
            Destroy(this.gameObject);
        }
    }
}
