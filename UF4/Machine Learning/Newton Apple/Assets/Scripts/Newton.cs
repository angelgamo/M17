using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using NaughtyAttributes;

public class Newton : Agent
{
	Rigidbody2D body;
	public Collider2D gridArea;
	Bounds bounds;
	public float speed;
	[HorizontalLine(color: EColor.Gray)]
	public TMPro.TextMeshPro points;
	public TMPro.TextMeshPro pointsMax;
	int pointsAccumulated;
	int pointsAccumulatedMax;
	public Transform apple;

	public override void Initialize()
	{
		body = GetComponent<Rigidbody2D>();

		bounds = gridArea.bounds;
		RandomizePosition();

		points.text = pointsAccumulated.ToString();
		pointsMax.text = pointsAccumulatedMax.ToString();

		apple.GetComponent<Apple>().appleHitFloor += OnLoose;
	}

	public override void OnEpisodeBegin()
	{
		RandomizePosition();
		pointsAccumulated = 0;
		points.text = pointsAccumulated.ToString();
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		float distanceX = apple.transform.position.x - transform.position.x;
		float distanceY = apple.transform.position.y - transform.position.y;
		sensor.AddObservation(distanceY);
		sensor.AddObservation(distanceX);
		sensor.AddObservation(body.velocity.x);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		ActionSegment<float> continuosActions = actionsOut.ContinuousActions;

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			continuosActions[0] = -1f;
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			continuosActions[0] = 1f;
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		float velocity = body.velocity.x + speed * actions.ContinuousActions[0] * Time.deltaTime;

		body.velocity = new Vector2(velocity, 0);
	}

	private void Update()
	{
		RequestDecision();
	}

	public void ManualInput()
	{
		float vel = 0f;

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			vel = -1f;
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			vel = 1f;

		vel *= speed * Time.deltaTime;
		body.velocity = new Vector2(vel, 0);
	}

	public void RandomizePosition()
	{
		body.velocity = Vector2.zero;

		// Pick a random position inside the bounds
		float x = Random.Range(bounds.min.x, bounds.max.x);

		// Round the values to ensure it aligns with the grid
		x = Mathf.Round(x);

		transform.position = new Vector2(x, bounds.center.y);
	}

	public void OnLoose()
	{
		AddReward(-1f);
		EndEpisode();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Apple"))
		{
			AddReward(.1f);
			AddReward(.1f / (Mathf.Abs(body.velocity.x) + 1f));
			pointsAccumulated++;
			points.text = pointsAccumulated.ToString();
			if (pointsAccumulatedMax < pointsAccumulated)
			{
				pointsAccumulatedMax = pointsAccumulated;
				pointsMax.text = pointsAccumulatedMax.ToString();
			}
		}
	}
}
