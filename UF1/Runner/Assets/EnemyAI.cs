using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletDelay;
    public int healtPoints;
    public int scoreValue;
    private PlayerController pc;
    public GameObject bonus;

    public enum Mode {OneShoot, TripleShoot, QuadShoot, NoPredict, StrafeShoot};
    public Mode aiMode;

    private int strafeAngle;
    private bool strafeWay;
    private int side;

    private bool cooldownBool;
    private float cooldownOffTime;
    private float cooldownOnTime;

    public bool canShoot = false;

    public GameObject explosion;

    void Start() // inicializa variables
    {
        player = GameObject.Find("Player").transform;
        pc = FindObjectOfType<PlayerController>();
        strafeAngle = 20;
        strafeWay = true;
        side = 1;
        cooldownBool = false;
        cooldownOnTime = 0.7f;
        cooldownOffTime = 1.5f;
        switch (aiMode)
        {
            case Mode.OneShoot:
                InvokeRepeating("OneShoot", 0, bulletDelay);
                break;
            case Mode.TripleShoot:
                InvokeRepeating("TripleShoot", 0, bulletDelay);
                break;
            case Mode.QuadShoot:
                InvokeRepeating("QuadShoot", 0, bulletDelay);
                break;
            case Mode.NoPredict:
                InvokeRepeating("NoPredict", 0, bulletDelay);
                Invoke("changeBoolTrue", cooldownOnTime);
                break;
            case Mode.StrafeShoot:
                InvokeRepeating("StrafeShoot", 0, bulletDelay);
                break;
        }
        this.healtPoints += pc.difficult;
    }

    private void Update() // calcula si dispara hacia derecha o izquierda
    {
        if (player == null)
        {
            return;
        }
        if (player.position.x > this.transform.position.x)
        {
            side = -1;
        }
        else
        {
            side = 1;
        }
    }

    private void OnDestroy() // salta animacion explotar al morir
    {
        Instantiate(explosion, transform.position, transform.rotation);
    }

    private void NoPredict() // si puede disparar calcula trayectoria de la bala hacia el personaje y intancia bala
    {
        if (cooldownBool || player == null || !canShoot)
        {
            return;
        }
        GameObject bulletShooted =  Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted.GetComponent<Rigidbody2D>().AddForce((player.transform.position + new Vector3(0,Random.Range(-.5f,.5f),0) - bulletShooted.transform.position).normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    private void TripleShoot() // si puede disparar calcula trayectoria de la bala y la instancia
    {
        if (!canShoot)
        {
            return;
        }
        GameObject bulletShooted1 = Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted1.GetComponent<Rigidbody2D>().AddForce(new Vector3((side * -10), 0, 0).normalized * bulletSpeed, ForceMode2D.Impulse);
        GameObject bulletShooted2 = Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted2.GetComponent<Rigidbody2D>().AddForce(new Vector3((side * -10), 3, 0).normalized * bulletSpeed, ForceMode2D.Impulse);
        GameObject bulletShooted3 = Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted3.GetComponent<Rigidbody2D>().AddForce(new Vector3((side * -10), -3, 0).normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    private void QuadShoot() // si puede disparar calcula trayectoria de la bala y la instancia
    {
        if (!canShoot)
        {
            return;
        }
        GameObject bulletShooted1 = Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted1.GetComponent<Rigidbody2D>().AddForce(new Vector3((side * -10), 3, 0).normalized * bulletSpeed, ForceMode2D.Impulse);
        GameObject bulletShooted2 = Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted2.GetComponent<Rigidbody2D>().AddForce(new Vector3((side * -10), 1, 0).normalized * bulletSpeed, ForceMode2D.Impulse);
        GameObject bulletShooted3 = Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted3.GetComponent<Rigidbody2D>().AddForce(new Vector3((side * -10), -1, 0).normalized * bulletSpeed, ForceMode2D.Impulse);
        GameObject bulletShooted4 = Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted4.GetComponent<Rigidbody2D>().AddForce(new Vector3((side * -10), -3, 0).normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OneShoot() // si puede disparar trayectoria de la bala y la instancia
    {
        if (!canShoot)
        {
            return;
        }
        GameObject bulletShooted = Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted.GetComponent<Rigidbody2D>().AddForce(new Vector3(bulletShooted.transform.position.x - (side * 10), 0, 0).normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    private void StrafeShoot() // si puede disparar calcula trayectoria de la bala y la instancia
    {
        if (!canShoot)
        {
            return;
        }
        GameObject bulletShooted = Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletShooted.GetComponent<Rigidbody2D>().AddForce(new Vector3(side * -strafeAngle, strafeAngle - 90, 0).normalized * bulletSpeed, ForceMode2D.Impulse);
        strafeWay = strafeWay && strafeAngle == 70 ? false : !strafeWay && strafeAngle == 20 ? true : strafeWay;
        strafeAngle += strafeWay ? 10 : -10;
    }

    private void changeBoolFalse()
    {
        cooldownBool = false;
        Invoke("changeBoolTrue", cooldownOnTime);
    }

    private void changeBoolTrue()
    {
        cooldownBool = true;
        Invoke("changeBoolFalse", cooldownOffTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BulletAlly" && canShoot) // si esta activo recibe daño y en provabilidad 1 entre 5 spawnea un bonus
        {
            this.healtPoints--;
            Destroy(collision.gameObject);
            if (this.healtPoints <= 0)
            {
                pc.score += scoreValue;
                if (Random.Range(0, 5) == 0)
                {
                    Instantiate(bonus, this.transform.position, this.transform.rotation);
                }
                Destroy(this.gameObject);
            }
        }
        if (collision.tag == "EnemyDespawn") // elimina si se sale del margen de la pantalla
        {
            Destroy(this.gameObject);
        }
        if (collision.tag == "EnemyNoShoot") // activa el disparo y recibir daño
        {
            this.canShoot = true;
        }
    }

    private void OnDrawGizmosSelected() // gizmos debugging
    {
        switch (aiMode)
        {
            case Mode.OneShoot:
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-20, 0, 0));
                break;
            case Mode.TripleShoot:
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-10, -3, 0).normalized * 20);
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-10, 0, 0).normalized * 20);
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-10, 3, 0).normalized * 20);
                break;
            case Mode.QuadShoot:
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-10, 3, 0).normalized * 20);
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-10, 1, 0).normalized * 20);
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-10, -1, 0).normalized * 20);
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-10, -3, 0).normalized * 20);
                break;
            case Mode.StrafeShoot:
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-20, -70, 0));
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-30, -60, 0));
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-40, -50, 0));
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-50, -40, 0));
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-60, -30, 0));
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-70, -20, 0));
                break;
        }
    }
}
