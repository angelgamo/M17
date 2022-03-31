using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public class Food : MonoBehaviour
{
    public Collider2D gridArea;
    [Expandable] public FoodRespawn foodRespawn;

    private void Start()
    {
        RandomizePosition();
    }

    [Button]
    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        // Pick a random position inside the bounds
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        // Round the values to ensure it aligns with the grid
        x = Mathf.Round(x);
        y = Mathf.Round(y);

        transform.position = new Vector2(x, y);

        StopCoroutine("AutoRespawn");
        StartCoroutine("AutoRespawn");
    }

    IEnumerator AutoRespawn()
	{
        yield return new WaitForSecondsRealtime(foodRespawn.timeToRespawn);
        RandomizePosition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RandomizePosition();
    }
}
