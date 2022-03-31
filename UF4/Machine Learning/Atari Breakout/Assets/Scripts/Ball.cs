using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using NaughtyAttributes;
using System.Collections;

public class Ball : Agent
{
	public float ballSpeed;
	[HorizontalLine]
	public Transform paddle;
	public float platformSpeed;
	public float rangeX;
	[HorizontalLine]
	public Transform blocks;
	[HorizontalLine]
	Rigidbody2D rb;
	System.Random random;
	public int blocksLeft;

	public override void Initialize()
	{
		rb = GetComponent<Rigidbody2D>();
		random = new System.Random((int)System.DateTime.Now.Ticks);
	}

	public override void OnEpisodeBegin()
	{
		float angle = .5f * Mathf.PI * ((float)random.NextDouble() - .5f) + Mathf.PI * .5f;
		transform.localPosition = new Vector3(0f, -9f);
		rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * ballSpeed;
		StartCoroutine(StartBlocks());
		blocksLeft = blocks.childCount;
	}

	IEnumerator StartBlocks()
	{
		yield return null;
		foreach (Transform block in blocks)
			block.gameObject.SetActive(true);
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		float distanceX = transform.localPosition.x - paddle.localPosition.x;
		if (Mathf.Abs(distanceX) < paddle.localScale.x * .3f)
			distanceX =  0f;
		sensor.AddObservation(distanceX);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

		discreteActions[0] = 0;

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			discreteActions[0] = 1;
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			discreteActions[0] = 2;
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		if (actions.DiscreteActions[0] == 1)
			paddle.localPosition = new Vector2(Mathf.Clamp(paddle.localPosition.x - platformSpeed * Time.deltaTime, -rangeX, rangeX), paddle.localPosition.y);
		else if (actions.DiscreteActions[0] == 2)
			paddle.localPosition = new Vector2(Mathf.Clamp(paddle.localPosition.x + platformSpeed * Time.deltaTime, -rangeX, rangeX), paddle.localPosition.y);
	}

	private void Update()
	{
		RequestDecision();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.CompareTag("Paddle"))
		{
			var angle = Mathf.PI * .6666666f * ((paddle.localPosition.x - transform.localPosition.x) / paddle.localScale.x) + Mathf.PI * .5f;
			rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * ballSpeed;
			AddReward(.1f);
		}
		else if (collision.CompareTag("Loose"))
		{
			AddReward(-1f);
			EndEpisode();
		}
			
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Block"))
		{
			StartCoroutine(Delay(collision.gameObject));
			if (--blocksLeft <= 0)
				EndEpisode();
		}
	}

	IEnumerator Delay(GameObject block)
	{
		yield return null;
		block.SetActive(false);
	}
}
