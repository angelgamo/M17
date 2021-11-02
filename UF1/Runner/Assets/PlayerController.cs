using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int spd;
    private Vector3 movement;
    public int score;
    public int lives;
    public GameObject bullet;
    public int bulletSpd;
    public GameObject laser;

    private float fireElapsedTime = 0;
    public float fireDelay;

    public enum ShootType { ball, laser };
    public ShootType shootType;
    private int shootLevel;

    private SpriteRenderer sr;
    private bool invencible;
    private bool blinkOn;
    private int blinkCount;

    public int difficult;
    public int bonusPoints;

    public GameObject explosion;

    void Start() // se inicializan variables
    {
        this.difficult = 0;
        this.invencible = false;
        this.blinkOn = true;
        this.sr = GetComponent<SpriteRenderer>();
        this.score = 0;
        this.lives = 3;
        this.shootLevel = 1;
        InvokeRepeating("incrementDifficulty", 0, 2);
    }

    void Update() // calculo de movimiento y disparo
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        movement.Normalize();


        fireElapsedTime += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && fireElapsedTime >= fireDelay)
        {
            fireElapsedTime = 0;
            shoot();
        }
    }

    private void OnDestroy() // cuando muere, salta animacion de explosion
    {
        Instantiate(explosion, transform.position, transform.rotation);
    }

    private void incrementDifficulty() // cada 500 puntos de score, todos los enemigos tienen +1 de vida
    {
        this.difficult = this.score / 500;
    }

    public Vector2 getPointAngle(float angle, float distance) // calculo de disparo segun angulo para gizmos
    {
        angle = angle * Mathf.PI / 180;
        return new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));
    }
    public Vector2 getPointAngle(Vector2 position, float angle, float distance) // calculo de disparo segun angulo
    {
        angle = angle * Mathf.PI / 180;
        return new Vector2(distance * Mathf.Cos(angle) + position.x, distance * Mathf.Sin(angle) + position.y);
    }

    private void FixedUpdate()
    {
        this.GetComponent<Rigidbody2D>().velocity = spd * movement * Time.deltaTime; // movimiento
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BulletEnemy" || collision.transform.tag == "Enemy")
        {
            if (this.invencible) // si es invulnerable no recibe daño y destruye al enemigo
            {
                Destroy(collision.gameObject);
            }
            else // sino pierde vida, y salta animacion invulnerable parpadeando, y destruye al enemigo
            {
                this.lives--;
                hit();
                Destroy(collision.gameObject);
            }
            if (this.lives <= 0) // si el jugador se queda sin vida, guarda la puntuacion, destruye al player, canvia de escena
            {
                PlayerPrefs.SetInt("Score", this.score);
                Destroy(this.gameObject);
                SceneManager.LoadScene("GameOver");
            }
        }
        if (collision.transform.tag == "Bonus") // si colisiona con el bonus suma puntos, y aumenta el nivel de disparo
        {
            Bonus b = collision.GetComponent<Bonus>();
            this.score += bonusPoints;
            if (this.shootType == b.getShootType())
            {
                shootUpgrade();
            }else // si canvia de tipo de disparo, disminuye el nivel a 1
            {
                this.shootType = b.getShootType();
                shootDowngrade();
                shootDowngrade();
            }
            Destroy(collision.gameObject);
        }
    }

    private void hit() // hace invulnerable y empieza a parpadear
    {
        shootDowngrade();
        invencible = true;
        blinkCount = 10;
        InvokeRepeating("blink", 0, 0.3f);
    }

    private void blink() // parpadea (alpha color) durante un tiempo, vuelve a hacer vulnerable al terminar el tiempo
    {
        Color color = sr.color;
        if (blinkOn)
        {
            color.a = 0.2f;
            blinkOn = !blinkOn;
        }
        else
        {
            color.a = 1;
            blinkOn = !blinkOn;
        }
        sr.color = color;
        if (--blinkCount == 0) 
        {
            CancelInvoke("blink");
            this.invencible = false;
        }

    }

    private void shoot() // dispara segun el tipo de disparo
    {
        if (this.shootType == ShootType.ball)
        {
            shootBall();
        }else if (this.shootType == ShootType.laser)
        {
            shootLaser();
            shootLaser();
            shootLaser();
        }
    }

    private void shootUpgrade() // subir nivel disparo
    {
        this.shootLevel++;
        shootLevel = shootLevel > 3 ? 3 : shootLevel;
    }

    private void shootDowngrade() // bajar nivel disparo
    {
        this.shootLevel--;
        shootLevel = shootLevel < 1 ? 1 : shootLevel;
    }

    private void shootBall() // disparos basicos
    {
        if(this.shootLevel == 1)
        {
            GameObject bulletShoted = Instantiate(bullet, transform.position, transform.rotation);
            bulletShoted.GetComponent<Rigidbody2D>().velocity = new Vector3(1, 0, 0).normalized * bulletSpd;
        }
        else if (this.shootLevel == 2)
        {
            GameObject bulletShoted1 = Instantiate(bullet, transform.position, transform.rotation);
            GameObject bulletShoted2 = Instantiate(bullet, transform.position, transform.rotation);
            GameObject bulletShoted3 = Instantiate(bullet, transform.position, transform.rotation);
            bulletShoted1.GetComponent<Rigidbody2D>().velocity = new Vector3(10, 4, 0).normalized * bulletSpd;
            bulletShoted2.GetComponent<Rigidbody2D>().velocity = new Vector3(10, 0, 0).normalized * bulletSpd;
            bulletShoted3.GetComponent<Rigidbody2D>().velocity = new Vector3(10, -4, 0).normalized * bulletSpd;
        }
        else if(this.shootLevel == 3)
        {
            GameObject bulletShoted1 = Instantiate(bullet, transform.position, transform.rotation);
            GameObject bulletShoted2 = Instantiate(bullet, transform.position, transform.rotation);
            GameObject bulletShoted3 = Instantiate(bullet, transform.position, transform.rotation);
            GameObject bulletShoted4 = Instantiate(bullet, transform.position, transform.rotation);
            GameObject bulletShoted5 = Instantiate(bullet, transform.position, transform.rotation);
            GameObject bulletShoted6 = Instantiate(bullet, transform.position, transform.rotation);
            GameObject bulletShoted7 = Instantiate(bullet, transform.position, transform.rotation);
            bulletShoted1.GetComponent<Rigidbody2D>().velocity = getPointAngle(60, bulletSpd);
            bulletShoted2.GetComponent<Rigidbody2D>().velocity = getPointAngle(40, bulletSpd);
            bulletShoted3.GetComponent<Rigidbody2D>().velocity = getPointAngle(20, bulletSpd);
            bulletShoted4.GetComponent<Rigidbody2D>().velocity = getPointAngle(0, bulletSpd);
            bulletShoted5.GetComponent<Rigidbody2D>().velocity = getPointAngle(-20, bulletSpd);
            bulletShoted6.GetComponent<Rigidbody2D>().velocity = getPointAngle(-40, bulletSpd);
            bulletShoted7.GetComponent<Rigidbody2D>().velocity = getPointAngle(-60, bulletSpd);
        }
    }

    private void shootLaser() // disparos laser
    {
        if (this.shootLevel == 1)
        {
            GameObject LaserShoted = Instantiate(laser, transform.position + new Vector3(3, 0, 0), transform.rotation);
            LaserShoted.transform.parent = transform;
        }
        else if (this.shootLevel == 2)
        {
            GameObject LaserShoted1 = Instantiate(laser, transform.position + new Vector3(3, .2f, 0), transform.rotation);
            GameObject LaserShoted2 = Instantiate(laser, transform.position + new Vector3(3, -.2f, 0), transform.rotation);
            LaserShoted1.transform.parent = transform;
            LaserShoted2.transform.parent = transform;
        }
        else if (this.shootLevel == 3)
        {
            GameObject LaserShoted1 = Instantiate(laser, transform.position + new Vector3(3, 0, 0), transform.rotation);
            GameObject LaserShoted2 = Instantiate(laser, transform.position + new Vector3(3, .4f, 0), transform.rotation);
            GameObject LaserShoted3 = Instantiate(laser, transform.position + new Vector3(3, -.4f, 0), transform.rotation);
            LaserShoted1.transform.parent = transform;
            LaserShoted2.transform.parent = transform;
            LaserShoted3.transform.parent = transform;
        }
    }

    private void OnDrawGizmosSelected() // gizmos debugging
    {
        Gizmos.color = Color.green;
        switch (this.shootType)
        {
            case ShootType.ball:
                if (this.shootLevel >= 1)
                {
                    Gizmos.DrawLine(this.transform.position, getPointAngle(this.transform.position, 0, 10));
                }
                if (this.shootLevel >= 2)
                {
                    Gizmos.DrawLine(this.transform.position, getPointAngle(this.transform.position, 20, 10));
                    Gizmos.DrawLine(this.transform.position, getPointAngle(this.transform.position, -20, 10));
                }
                if (this.shootLevel >= 3)
                {
                    Gizmos.DrawLine(this.transform.position, getPointAngle(this.transform.position, 60, 10));
                    Gizmos.DrawLine(this.transform.position, getPointAngle(this.transform.position, 40, 10));
                    Gizmos.DrawLine(this.transform.position, getPointAngle(this.transform.position, -40, 10));
                    Gizmos.DrawLine(this.transform.position, getPointAngle(this.transform.position, -60, 10));
                }
                break;
            case ShootType.laser:
                if (this.shootLevel == 1)
                {
                    Gizmos.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(this.transform.position.x + 6, this.transform.position.y));
                }else if (this.shootLevel == 2)
                {
                    Gizmos.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y + .2f), new Vector2(this.transform.position.x + 6, this.transform.position.y + .2f));
                    Gizmos.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y - .2f), new Vector2(this.transform.position.x + 6, this.transform.position.y - .2f));
                }
                else if (this.shootLevel == 3)
                {
                    Gizmos.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(this.transform.position.x + 6, this.transform.position.y));
                    Gizmos.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y + .4f), new Vector2(this.transform.position.x + 6, this.transform.position.y + .4f));
                    Gizmos.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y - .4f), new Vector2(this.transform.position.x + 6, this.transform.position.y - .4f));
                }
                break;
        }
    }
}
