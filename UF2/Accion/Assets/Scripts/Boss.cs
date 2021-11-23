using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public List<GameObject> bala;
    public int sp;
    public bool isDown;
    public Vector3 offsetCanon;
    public Vector3 offsetBomb;
    public Vector3 offsetDamagetime;
    public Vector3 initilaPosition;
    public Vector3 offsetBazoka;
    public GameObject canonBall;
    public float canonAngle;
    public float canonForce;
    public delegate void LoseLife();
    public event LoseLife OnLoseLife;

    void Start()
    {
        initilaPosition = transform.position;
        this.gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine("shoot");
        StartCoroutine("cannon");
        StartCoroutine("boomb");
        StartCoroutine(LerpPosition(2));

        GetComponent<HpManager>().onDeath += Death;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDown)
        {
            this.gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        }
        else 
        {
            this.gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        }
    }
    IEnumerator LerpPosition(float duration)
    {
        Vector3 target;
        if (!isDown) 
        {
            this.GetComponent<AudioSource>().pitch = 1f;
            yield return new WaitForSeconds(10);
        }
        else 
        {
            this.GetComponent<AudioSource>().pitch = 0.75f;
            yield return new WaitForSeconds(5);
        }
        
        if (isDown)
        {
            target = initilaPosition;
            isDown = false;
        }
        else 
        {
            target = offsetDamagetime;
            isDown = true;
        }
        float time = 0;
        Vector2 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        StartCoroutine(LerpPosition(duration));

    }

    IEnumerator cannon() 
    {
        while (true)
        {
            if (!isDown) {
                shootCanon();
            }
            yield return new WaitForSeconds(Random.Range(2f,5f));
        }
    }
    IEnumerator shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(6f);
            if (!isDown)
            {
                shootBullet();
                yield return new WaitForSeconds(0.5f);
                shootBullet();
                yield return new WaitForSeconds(0.5f);
                shootBullet();
            }
        }
    }
    IEnumerator boomb() {
        float time = Random.Range(5, 10);
        yield return new WaitForSeconds(time);
        if (!isDown) {
            GameObject newBomba = Instantiate(bala[1], transform.position + offsetBomb, Quaternion.identity);
            newBomba.transform.rotation = Quaternion.Euler(0, 0, 107);
            newBomba.GetComponent<Rigidbody2D>().velocity = transform.up * sp * 7;
        }
        StartCoroutine("boomb");
    }

    void shootCanon()
    {
        GameObject canonBallClone = Instantiate(canonBall, transform.position + offsetCanon, Quaternion.identity);
        canonBallClone.GetComponent<Rigidbody2D>().AddForce(getPointAngle(canonAngle,  canonForce));
        Destroy(canonBallClone, 5f);
    }
    void shootBullet() {
        GameObject newBala = Instantiate(bala[0], transform.position + offsetBazoka, Quaternion.identity);
        newBala.transform.eulerAngles = new Vector3(0, 0, -90);
        newBala.GetComponent<Rigidbody2D>().velocity = newBala.transform.up * sp;
        Destroy(newBala, 3f);
        GameObject newBala2 = Instantiate(bala[0], transform.position + offsetBazoka, Quaternion.identity);
        newBala2.transform.eulerAngles = new Vector3(0, 0, -70);
        newBala2.GetComponent<Rigidbody2D>().velocity = newBala2.transform.up * sp;
        Destroy(newBala2, 3f);
        GameObject newBala3 = Instantiate(bala[0], transform.position + offsetBazoka, Quaternion.identity);
        newBala3.transform.eulerAngles = new Vector3(0, 0, -110);
        newBala3.GetComponent<Rigidbody2D>().velocity = newBala3.transform.up * sp;
        Destroy(newBala3, 3f);
    }

    Vector2 getPointAngle(float angle, float distance)
    {
        angle = angle * Mathf.PI / 180;
        return new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));
    }
    /*
    private void OnDisable()
    {
        SceneManager.LoadScene(sceneName: "End");
    }*/
    /*
    private void OnDestroy()
    {
        SceneManager.LoadScene(sceneName: "End");
    }*/

    void Death()
    {
        SceneManager.LoadScene(sceneName: "End");
    }
}
