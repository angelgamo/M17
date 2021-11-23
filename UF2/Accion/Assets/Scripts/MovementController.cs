using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    Vector2 movement;
    float movementX;
    public float speed = 5;
    bool jump;
    public float jumpSpeed = 10;

    public bool isGrounded;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        movementX = Input.GetKey("a") ? -1 : Input.GetKey("d") ? 1: 0;
        jump = Input.GetKeyDown("w") && isGrounded ? true : jump;

        // OnGround
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            isGrounded = true;
        }
        else isGrounded = false;

        anim.SetBool("Shield", Input.GetKey("l"));
        //if (Input.GetKeyDown("l")) anim.SetBool("Shield", !anim.GetBool("Shield"));
    }

    private void FixedUpdate()
    {
        movement.x = movementX * speed * Time.deltaTime;
        movement.y = GetComponent<Rigidbody2D>().velocity.y;
        GetComponent<Rigidbody2D>().velocity = movement;
        if (jump)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpSpeed);
            jump = false;
        }

        if (movementX != 0)
            anim.SetFloat("LateralDirection", movementX);
        anim.SetFloat("LateralMovement", Mathf.Abs(movementX));

        if (movement.y < -0.01 || movement.y > 0.01)
        {
            anim.SetFloat("VerticalDirection", movement.y > 0 ? 1 : -1);
            anim.SetFloat("VerticalMovement", Mathf.Abs(movement.y));
        }
        else
        {
            anim.SetFloat("VerticalDirection", 0);
            anim.SetFloat("VerticalMovement", 0);
        }
    }

  
}
