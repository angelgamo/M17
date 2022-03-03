using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

public class PlayerController2 : MonoBehaviour
{
	CharacterController ch;
	new Transform camera;

	[Header("Movement")]
	public float moveForce = 10000f;
	[Range(0f, 1f)] public float airControl = 0.6f;
	public float jumpForce;

	[Header("Clamp Velocity")]
	public float maxVelocity = 7.5f;
	public float maxSprintVelocity = 12.5f;
	public float maxAirVelocity = 20f;

	[Header("Gravity")]
	public float gravityOnAir = 19.62f;
	public float gravityOnGround = 1f;

	[Header("Drag")]
	public float dragOnAir = 1;
	public float dragOnGround = 5f;

	[Header("Keybinds")]
	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode sprintKey = KeyCode.LeftShift;

	[Header("Inputs")]
	[ReadOnly] public Vector3 movement;
	[ReadOnly] public bool jump;
	[ReadOnly] public bool sprint;

	[Header("Physics")]
	public float mass = 3f;
	public LayerMask groundMask;
	[ReadOnly] public float drag = 0f;
	[ReadOnly, Tooltip("Vertical speed/ gravity")] public float vSpeed = 0f;
	[ReadOnly] public Vector3 velocity;
	[ReadOnly] public bool slope;
	RaycastHit slopeHit;

	private void Awake()
	{
		ch = GetComponent<CharacterController>();
		camera = Camera.main.transform;
	}

	public void Update()
	{
		InputHandler();
	}

	public void FixedUpdate()
	{
		Rotate();
		Move();
		CapVelcity();
		Jump();
		ApplyForces();
	}

	void InputHandler()
	{
		// Movement
		movement = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
		sprint = Input.GetKey(sprintKey);

		// Jump
		if (Input.GetKeyDown(jumpKey))
		{
			StopCoroutine("QueueJump");
			StartCoroutine("QueueJump");
		}
	}

	IEnumerator QueueJump()
	{
		jump = true;
		yield return new WaitForSeconds(0.15f);
		jump = false;
	}

	void Move()
	{
		if (movement.magnitude < 0.01f)
			return;
		
		if (ch.isGrounded)
			if (onSlope())
			{
				Vector3 slopeMovement = Vector3.ProjectOnPlane(movement, slopeHit.normal).normalized;
				if (slopeMovement.y > 0)
					slopeMovement.y = 0;
				AddForce(slopeMovement * moveForce * Time.deltaTime);
			}
			else
				AddForce(movement.normalized * moveForce * Time.deltaTime);
		else
			AddForce(movement.normalized * moveForce * Time.deltaTime * airControl);
	}

	void CapVelcity()
	{
		Vector3 vel = new Vector3(velocity.x, 0, velocity.z);
		if (ch.isGrounded)
			if (sprint)
				vel = Vector3.ClampMagnitude(vel, maxSprintVelocity);
			else
				vel = Vector3.ClampMagnitude(vel, maxVelocity);
		else
			vel = Vector3.ClampMagnitude(vel, maxAirVelocity);

		vel.y = velocity.y;
		velocity = vel;
	}

	void Jump()
	{
		if (!jump || !ch.isGrounded)
			return;

		velocity.y = 0;
		AddForce(transform.up * jumpForce, ForceMode.Impulse);
		jump = false;
	}

	void Gravity()
	{
		if (ch.isGrounded)
			vSpeed = mass * gravityOnGround * Time.deltaTime;
		else
			vSpeed += mass * gravityOnAir * Time.deltaTime;
	}

	void Drag()
	{
		if (ch.isGrounded)
			drag = dragOnGround;
		else
			drag = dragOnAir;

		AddForce(-velocity * drag * mass);
	}

	void Rotate()
	{
		var rot = new Vector3(0, camera.transform.rotation.eulerAngles.y, 0);
		transform.rotation = Quaternion.Euler(rot);
	}

	bool onSlope()
	{
		if (Physics.Raycast(transform.position, -transform.up, out slopeHit, ch.height * .5f +.2f, groundMask))
			if (slopeHit.normal != Vector3.up)
				return true;
		return false;
	}

	#region Forces
	public void AddForce(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddForce_Injected(force, mode);
	}
	public void AddForce(Vector3 force)
	{
		AddForce(force, ForceMode.Force);
	}
	public void AddForce(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddForce(new Vector3(x, y, z), mode);
	}
	public void AddForce(float x, float y, float z)
	{
		AddForce(new Vector3(x, y, z), ForceMode.Force);
	}

	public void AddForce_Injected(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		if (mode == ForceMode.Impulse)
			velocity += mass * force;
		else if (mode == ForceMode.Force)
			velocity += mass * force * Time.fixedDeltaTime;
		else if (mode == ForceMode.VelocityChange)
			velocity += force;
		else if (mode == ForceMode.Acceleration)
			velocity += force * Time.fixedDeltaTime;
	}
	#endregion

	void ApplyForces()
	{
		Vector3 force = velocity;
		force.y -= vSpeed; // Add Gravity

		if (force.magnitude > 0.15f)
			ch.Move(force * Time.deltaTime);

		Gravity(); // Recalculate gravity
		Drag(); // Update drag
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		if (ch != null)
			Gizmos.DrawRay(transform.position, -transform.up * (ch.height * .5f + .2f));
	}

	private void OnCollisionEnter(Collision collision)
	{
		print(collision.transform.name);
	}

	private void OnCollisionStay(Collision collision)
	{
		print(collision.transform.name);
	}
}

#region ReadOnly
public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property,
											GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	public override void OnGUI(Rect position,
							   SerializedProperty property,
							   GUIContent label)
	{
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}
}
#endregion