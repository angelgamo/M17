using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMove : MonoBehaviour
{
	Vector3 desiredVelocity;
	Vector3 velocity;
	[SerializeField, Range(0f, 100f)]
	float maxAcceleration = 10f, maxAirAcceleration = 1f;
	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f;
	Rigidbody body;
	bool desiredJump;
	[SerializeField, Range(0f, 10f)]
	float jumpHeight = 2f;
	int groundContactCount;
	bool OnGround => groundContactCount > 0;
	[SerializeField, Range(0, 5)]
	int maxAirJumps = 0;
	int jumpPhase;
	[SerializeField, Range(0f, 90f)]
	float maxGroundAngle = 25f;
	float minGroundDotProduct;
	Vector3 contactNormal;
	int stepsSinceLastGrounded, stepsSinceLastJump;
	[SerializeField, Range(0f, 100f)]
	float maxSnapSpeed = 100f;
	[SerializeField, Min(0f)]
	float probeDistance = 1f;
	[SerializeField]
	LayerMask probeMask = -1;

	// Start is called before the first frame update
	void Start()
    {
		
	}

    private void Awake()
    {
		body = GetComponent<Rigidbody>();
		OnValidate();
	}
    void Update()
	{
		desiredJump |= Input.GetButtonDown("Jump");

		Vector2 playerInput;
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);

		desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
		GetComponent<Renderer>().material.SetColor(
			"_Color", OnGround ? Color.black : Color.white
		);

	}

	void FixedUpdate()
	{
		UpdateState();	
		AdjustVelocity();
		if (desiredJump)
		{
			desiredJump = false;
			Jump();
		}

		body.velocity = velocity;
		ClearState();
	}
	void UpdateState()
	{
		stepsSinceLastGrounded += 1;
		stepsSinceLastJump += 1;
		velocity = body.velocity;
		if (OnGround || SnapToGround())
		{
			stepsSinceLastGrounded = 0;
			jumpPhase = 0;
			if (groundContactCount > 1)
			{
				contactNormal.Normalize();
			}
		}
		else
		{
			contactNormal = Vector3.up;
		}
	}
	void Jump()
	{
        if (OnGround || jumpPhase < maxAirJumps)
        {
			stepsSinceLastJump = 0;
			jumpPhase += 1;
			float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
			float alignedSpeed = Vector3.Dot(velocity, contactNormal);
			if (alignedSpeed > 0f)
			{
				jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
			}
			velocity += contactNormal * jumpSpeed;
		}
	}

	void OnCollisionStay(Collision collision)
	{
		EvaluateCollision(collision);
	}
	void OnCollisionExit(Collision collision)
	{
		EvaluateCollision(collision);
	}
	void EvaluateCollision(Collision collision) 
	{
		for (int i = 0; i < collision.contactCount; i++)
		{
			Vector3 normal = collision.GetContact(i).normal;
			//onGround |= normal.y >= 0.9f;
			if (normal.y >= minGroundDotProduct)
			{
				groundContactCount += 1;
				contactNormal += normal;
			}
		}
	}
	void OnValidate()
	{
		minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
	}
	Vector3 ProjectOnContactPlane(Vector3 vector)
	{
		return vector - contactNormal * Vector3.Dot(vector, contactNormal);
	}
	void AdjustVelocity()
	{
		Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
		Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

		float currentX = Vector3.Dot(velocity, xAxis);
		float currentZ = Vector3.Dot(velocity, zAxis);

		float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
		float maxSpeedChange = acceleration * Time.deltaTime;

		float newX =
			Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
		float newZ =
			Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);
		velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
	}
	void ClearState()
	{
		groundContactCount = 0;
		contactNormal = Vector3.zero;
	}
	bool SnapToGround()
	{
		if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2)
		{
			return false;
		}
		float speed = velocity.magnitude;
		if (speed > maxSnapSpeed)
		{
			return false;
		}
		if (!Physics.Raycast(body.position, Vector3.down, out RaycastHit hit, probeDistance, probeMask))
		{
			return false;
		}
		if (hit.normal.y < minGroundDotProduct)
		{
			return false;
		}
		groundContactCount = 1;
		contactNormal = hit.normal;
		float dot = Vector3.Dot(velocity, hit.normal);
		if (dot > 0f)
		{
			velocity = (velocity - hit.normal * dot).normalized * speed;
		}
		return true;
	}

}
