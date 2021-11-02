using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 movement;
    public float speed;
    public GameObject bullet;

    bool canShoot = true;
    public Vector3 bulletOffset;
    public Vector2 bulletVelocity;
    public float bulletDelay;

    public Sprite white;
    public Sprite black;
    public Sprite whiteBullet;
    public Sprite blackBullet;
    public Sprite specialBullet;
    public bool isWhite = true;

    public int energy;

    public bool isKeyboard;

    public delegate void playerDeath();
    public event playerDeath onPlayerDeath;

    private void Start()
    {
        this.tag = "white";
        isWhite = true;
        isKeyboard = true;

        if (PlayerPrefs.GetInt("color") % 2 != 0)
        {
            ChangeColor();
        }

        if (PlayerPrefs.GetInt("keyboardmouse") % 2 != 0)
        {
            isKeyboard = false;
        }
    }

    void Update()
    {
        if (isKeyboard)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.Space) && canShoot)
            {
                print("disparo");
                StartCoroutine(Shoot());
            }

            if (Input.GetKeyDown(KeyCode.C)) ChangeColor();
        }
        else
        {
            movement = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(movement.x, movement.y, transform.position.z);

            if (Input.GetMouseButton(0) && canShoot)
            {
                StartCoroutine(Shoot());
            }

            if (Input.GetMouseButtonDown(1)) ChangeColor();
        }
        
    }

    private void FixedUpdate()
    {
        if (isKeyboard) GetComponent<Rigidbody2D>().AddForce(movement * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if (collision.tag == this.tag)
            {
                energy++;
            }
            else
            {
                if (onPlayerDeath != null) onPlayerDeath.Invoke();
                //print("Game Over");
            }

            Destroy(collision.gameObject);
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        GameObject bulletClone = Instantiate(bullet, transform.position + bulletOffset, Quaternion.identity);
        bulletClone.tag = isWhite ? "white" : "black";
        bulletClone.layer = 6;
        bulletClone.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        if (!isWhite) bulletClone.GetComponent<SpriteRenderer>().sprite = blackBullet;
        yield return new WaitForSeconds(bulletDelay);
        canShoot = true;
    }

    void ChangeColor()
    {
        GetComponent<SpriteRenderer>().sprite = isWhite? black : white;
        isWhite = !isWhite;
        tag = isWhite ? "white" : "black";

        if (energy >= 4)
        {
            GameObject bulletClone = Instantiate(bullet, transform.position + bulletOffset, Quaternion.identity);
            bulletClone.tag = "special";
            bulletClone.layer = 6;
            bulletClone.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
            bulletClone.GetComponent<BulletController>().energy = energy / 4;
            bulletClone.GetComponent<SpriteRenderer>().sprite = specialBullet;
            energy = 0;
        }
    }
}
