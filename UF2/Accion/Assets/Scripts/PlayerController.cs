using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    public Escudo escudo;
    public float tempsMin = 0.1f;
    public float tempsMax = 1f;
    private string state = "-";
    bool tooSoon = false;
    bool isShielded = false;

    Vector3 targetThrust;

    public GameObject arrow;
    public Vector3 arrowSpawnPos;
    public Vector2 arrowVelocity;
    float cargaAtac;

    public delegate void RecibirDaño();
    public event RecibirDaño OnRecibirDaño;

    void Start()
    {
        anim = GetComponent<Animator>();
        GetComponent<HpManager>().onDeath += Death;
    }

    void Update()
    {
        if (Input.GetKeyDown("j"))
            StartCoroutine(comboNewInput("A"));
        else if (Input.GetKeyDown("k"))
            StartCoroutine(comboNewInput("B"));

        anim.SetBool("BDown", Input.GetKey("k"));
    }

    IEnumerator comboNewInput(string input)
    {
        if (!tooSoon)
        {
            switch (state)
            {
                case "-":
                    if (input == "A")
                    {
                        anim.SetTrigger("A");
                        anim.ResetTrigger("B");
                        state = "A";
                    }
                    else if (input == "B")
                    {
                        anim.ResetTrigger("A");
                        anim.SetTrigger("B");
                        //anim.SetBool("BDown", true);
                        state = "B";
                    }
                    break;
                case "A":
                    if (input == "A")
                    {
                        anim.SetTrigger("A");
                        anim.ResetTrigger("B");
                        state = "AA";
                    }
                    else if (input == "B")
                    {
                        anim.ResetTrigger("A");
                        anim.SetTrigger("B");
                        state = "-";
                    }
                    break;
                case "AA":
                    if (input == "A")
                    {
                        anim.SetTrigger("A");
                        anim.ResetTrigger("B");
                        state = "-";
                    }
                    else if (input == "B")
                    {
                        anim.ResetTrigger("A");
                        anim.SetTrigger("B");
                        state = "-";
                    }
                    break;
            }
        }


        string copystate = state;
        tooSoon = true;
        yield return new WaitForSeconds(tempsMin);
        tooSoon = false;
        yield return new WaitForSeconds(tempsMax - tempsMin);
        if (state == copystate && state != "-")
        {
            state = "-";
            anim.ResetTrigger("A");
        }
    }

    public IEnumerator cargaArrow()
    {
        StopCoroutine("comboNewInput");
        state = "B" + "->";
        cargaAtac = 0;
        while (Input.GetKey(KeyCode.K))
        { 
            if (cargaAtac < 1) cargaAtac+=0.2f;
            yield return new WaitForSeconds(0.16f);
        }
        state = "-";
    }


    public void trhust()
    {
        StartCoroutine(TrhustForce(2, 0.2f));
    }

    public void trhustLeft()
    {
        StartCoroutine(TrhustForce(-2, 0.2f));
    }

    public void megaTrhust()
    {
        StartCoroutine(TrhustForce(4, 0.2f));
    }

    public void megaTrhustLeft()
    {
        StartCoroutine(TrhustForce(-4, 0.2f));
    }

    IEnumerator TrhustForce(float distance, float duration)
    {
        targetThrust = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        

        float time = 0;
        Vector2 startPosition = transform.position;

        while (time < duration)
        {
            //transform.position = Vector2.Lerp(startPosition, targetThrust, time / duration);
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(startPosition, targetThrust, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void ShootArrow()
    {
        GameObject clone = Instantiate(arrow, transform.position + arrowSpawnPos, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().AddForce(arrowVelocity * cargaAtac, ForceMode2D.Impulse);
        Destroy(clone, 2f);
        cargaAtac = 0;
    }

    public void ShootArrowLeft()
    {
        GameObject clone = Instantiate(arrow, transform.position + arrowSpawnPos + (-Vector3.right), Quaternion.identity);
        clone.transform.Rotate(new Vector3(0, 0, 180));
        clone.GetComponent<Rigidbody2D>().AddForce(-arrowVelocity * cargaAtac, ForceMode2D.Impulse);
        Destroy(clone, 2f);
        cargaAtac = 0;
    }

    void bloquejar(float angle)
    {
        if(this.escudo.angle < angle || 360 - this.escudo.angle > 360 - angle)
        {
            OnRecibirDaño.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        GetComponent<MovementController>().enabled = true;
        if (collision.gameObject.layer == 9)
        {
            GetComponent<MovementController>().isGrounded = true;
        }*/

        if (isShielded)
        {
            Vector3 positionDifference = collision.gameObject.transform.position - gameObject.transform.position;
            float x = positionDifference.x;
            float y = positionDifference.y;
            float degrees = Mathf.Atan(y / x) * Mathf.Rad2Deg;

            bloquejar(degrees);
        }

    }
    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            GetComponent<MovementController>().isGrounded = false;
        }
    }*/
    /*
    private void OnDestroy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/

    void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
