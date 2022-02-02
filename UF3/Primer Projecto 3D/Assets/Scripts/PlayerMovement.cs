using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody rb;

    public float gravity = 9.81f;
    public Transform gravitySource;
    public Vector3 gravityVector;

	public float speed;
	public float speedRotation;
    public float maxSpeed;


    private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
        Vector3 movement = Vector3.zero;
		// Gravity Calc
		gravityVector = (gravitySource.position - transform.position).normalized;
		rb.AddForce(gravityVector * gravity);
		// Perpendicular player - suface
		transform.rotation = Quaternion.FromToRotation(transform.up, -gravityVector) * transform.rotation;
		

		// Movement Rotation
		if (Input.GetKey(KeyCode.A))
			transform.Rotate(Vector3.up, -speedRotation);
		else if (Input.GetKey(KeyCode.D))
			transform.Rotate(Vector3.up, speedRotation);

		// Movement
		if (Input.GetKey(KeyCode.W))
			rb.AddForce(transform.forward * speed);

     
    }
}
