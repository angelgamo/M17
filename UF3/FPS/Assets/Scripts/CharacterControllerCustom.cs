using UnityEngine;
using UnityEngine.Internal;


[RequireComponent(typeof(CharacterController))]
public class CharacterControllerCustom : MonoBehaviour
{
	CharacterController ch;

	public float mass = 3f;
	public float drag = 0f;
	public Vector3 force;
	public Vector3 velocity;
	public bool isGrounded;

	public void Awake()
	{
		ch = GetComponent<CharacterController>();
	}

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

	public void AddForce_Injected(Vector3 velocity, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		if (mode == ForceMode.Impulse)
			this.force += mass * velocity;
		else if (mode == ForceMode.Force)
			this.force += mass * velocity * Time.fixedDeltaTime;
		else if (mode == ForceMode.VelocityChange)
			this.force += velocity;
		else if (mode == ForceMode.Acceleration)
			this.force += velocity * Time.fixedDeltaTime;
	}

	public void Drag()
	{
		if (drag == 0 || Mathf.Abs(force.magnitude) > 0.01f)
			return;

		AddForce(-force * drag * mass);
	}

	void FixedUpdate()
	{
		if (force.magnitude > 0.2f)
			ch.Move(force * Time.deltaTime);

		force = Vector3.Lerp(force, Vector3.zero, 5 * Time.deltaTime);
		//Drag();
		velocity = ch.velocity;
		isGrounded = ch.isGrounded;
	}
}
