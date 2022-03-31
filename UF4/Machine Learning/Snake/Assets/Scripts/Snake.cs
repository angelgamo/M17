using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : Agent
{
    private List<Transform> segments = new List<Transform>();
    public Transform segmentPrefab;
    public Vector2 direction = Vector2.right;
    private Vector2 input;
    public int initialSize = 4;
    public float foodReward = .4f;

    Vector3 startPosition;

    //https://www.youtube.com/watch?v=vhiO4WsHA6c

    Queue<GameObject> pool;

	public override void Initialize()
	{
        startPosition = transform.position;
        pool = new Queue<GameObject>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        switch (actions.DiscreteActions[0])
        {
            case 0:
                if (input != Vector2.down)
                    input = Vector2.up;
                break;
            case 1:
                if (input != Vector2.up)
                    input = Vector2.down;
                break;
            case 2:
                if (input != Vector2.left)
                    input = Vector2.right;
                break;
            case 3:
                if (input != Vector2.right)
                    input = Vector2.left;
                break;
        }
        Move();
        AddReward(-0.025f);
    }

    public override void OnEpisodeBegin()
    {
        direction = Vector2.right;
        transform.position = startPosition;

        // Start at 1 to skip destroying the head
        for (int i = 1; i < segments.Count; i++)
        {
            //Destroy(segments[i].gameObject);
            ReturnSegment(segments[i]);
        }

        // Clear the list but add back this as the head
        segments.Clear();
        segments.Add(transform);

        // -1 since the head is already in the list
        for (int i = 0; i < initialSize - 1; i++)
        {
            Grow();
        }
    }

    private void Update()
    {
        RequestDecision();
    }

    void ManualInput()
	{
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && input != Vector2.down)
        {
            input = Vector2.up;
            Move();
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && input != Vector2.up)
        {
            input = Vector2.down;
            Move();
        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && input != Vector2.left)
        {
            input = Vector2.right;
            Move();
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && input != Vector2.right)
        {
            input = Vector2.left;
            Move();
        }
    }

    Transform GetSegment()
	{
        if (pool.Count > 0)
		{
            GameObject go = pool.Dequeue();
            go.SetActive(true);
            return go.transform;
		}
		else
            return Instantiate(segmentPrefab).transform;
	}

    void ReturnSegment(Transform segment)
	{
        GameObject go = segment.gameObject;
        go.SetActive(false);
        pool.Enqueue(go);
	}

    public void Move()
	{
        // Set the new direction based on the input
        if (input != Vector2.zero)
        {
            direction = input;
        }

        // Set each segment's position to be the same as the one it follows. We
        // must do this in reverse order so the position is set to the previous
        // position, otherwise they will all be stacked on top of each other.
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        // Move the snake in the direction it is facing
        // Round the values to ensure it aligns to the grid
        float x = Mathf.Round(transform.position.x) + direction.x;
        float y = Mathf.Round(transform.position.y) + direction.y;

        transform.position = new Vector2(x, y);
    }

    public void Grow()
    {
        //Transform segment = Instantiate(segmentPrefab);
        Transform segment = GetSegment();
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food")) {
            AddReward(foodReward);
            Grow();
        } else if (other.gameObject.CompareTag("Obstacle")) {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
