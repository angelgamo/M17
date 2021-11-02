using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 movement;
    public float speed;
    
    void Start()
    {
        movement = Vector2.zero;
    }

    
    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        movement = movement.normalized * speed * Time.deltaTime;
        this.GetComponent<Rigidbody2D>().velocity = movement;
    }

}
