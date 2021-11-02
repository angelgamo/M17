using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Vector3 movement;
    public float speed;

    void Start()
    {
        movement = Vector3.zero;
    }


    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
    }

    private void FixedUpdate()
    {
        movement = movement.normalized * speed * Time.deltaTime;
        this.transform.position += movement;
    }
}
