using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[HideInInspector] public Rigidbody rb;
	new Transform camera;

	[HideInInspector] public Vector3 movement;

	[Header("Movement")]
	public float moveForce;
	[Range(0f,1f)] public float airControl = 0.6f;
	public float jumpForce;
	bool jump;
	public bool slope;
	RaycastHit slopeHit;

	[Header("Clamp Velocity")]
	public float maxVelocity = 7.5f;
	public float maxSprintVelocity = 12.5f;
	public float maxAirVelocity = 20f;
	bool sprint;

	[Header("Gound")]
	public LayerMask groundMask;
	public Transform groundPosition;
	public float groundDistance;
	public bool isGrounded;

	[Header("Gravity")]
	public float gravityOnAir = 19.62f;
	public float gravityOnGround = 1f;

	[Header("Drag")]
	public float dragOnAir = 1;
	public float dragOnGround = 5f;

	[Header("Keybinds")]
	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode sprintKey = KeyCode.LeftShift;

	[Header("Other")]
	public Vector3 a;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		camera = Camera.main.transform;
	}

	private void Update()
	{
		movement = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");

		if (Input.GetKeyDown(jumpKey))
		{
			StopCoroutine("QueueJump");
			StartCoroutine("QueueJump");
		}

		sprint = Input.GetKey(sprintKey);
	}

	private void FixedUpdate()
	{
		isGrounded = IsGrounded();
		Rotate();
		Move();
		CapVelcity();
		Gravity();
		Jump();
		Drag();
	}

	void Rotate()
	{
		var rot = new Vector3(0, camera.transform.rotation.eulerAngles.y, 0);
		transform.rotation = Quaternion.Euler(rot);
	}

	void Move()
	{
		if (movement.magnitude < 0.01f)
			return;

		slope = onSlope();
		Vector3 slopeMovement = Vector3.zero;
		Vector3 slopeMovement2 = Vector3.zero;
		if (slope)
		{
			slopeMovement = Vector3.ProjectOnPlane(movement.normalized, slopeHit.normal).normalized;
			slopeMovement2 = (slopeMovement + a).normalized;
			Debug.DrawRay(transform.position, slopeMovement, Color.yellow, 5f);
			Debug.DrawRay(transform.position, slopeMovement2, Color.green, 5f);
		}
		Debug.DrawRay(transform.position, rb.velocity.normalized, Color.magenta, 5f);


		if (isGrounded)
			if (slope)
				rb.AddForce(slopeMovement2 * moveForce * Time.deltaTime);
			else
				rb.AddForce(movement.normalized * moveForce * Time.deltaTime);
		else
			rb.AddForce(movement.normalized * moveForce * Time.deltaTime * airControl);
	}

	IEnumerator QueueJump()
	{
		jump = true;
		yield return new WaitForSeconds(0.1f);
		jump = false;
	}

	void Jump()
	{
		if (!jump || !isGrounded)
			return;

		rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
		jump = false;
	}

	void Gravity()
	{
		if (isGrounded)
			rb.AddForce(transform.up * -gravityOnGround);
		else
			rb.AddForce(transform.up * -gravityOnAir);
	}

	void CapVelcity()
	{
		Vector3 vel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		if (isGrounded)
			if (sprint)
				vel = Vector3.ClampMagnitude(vel, maxSprintVelocity);
			else
				vel = Vector3.ClampMagnitude(vel, maxVelocity);
		else
			vel = Vector3.ClampMagnitude(vel, maxAirVelocity);

		vel.y = rb.velocity.y;
		rb.velocity = vel;
	}

	void Drag()
	{
		if (isGrounded)
			rb.drag = dragOnGround;
		else
			rb.drag = dragOnAir;
	}

	bool IsGrounded()
	{
		return Physics.OverlapSphere(groundPosition.position, groundDistance, groundMask).Length > 0;
	}

	bool onSlope()
	{
		if (Physics.Raycast(groundPosition.position, -transform.up, out slopeHit, groundDistance * 2, groundMask))
			if (slopeHit.normal != Vector3.up)
				return true;
		return false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(groundPosition.position, groundDistance);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(groundPosition.position, -transform.up * groundDistance * 2);
	}
}
