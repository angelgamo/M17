using UnityEngine;

public class Apple : MonoBehaviour
{
    Rigidbody2D rb;
    public Collider2D gridArea;
    Bounds bounds;
    public delegate void AppleHitFloor();
    public AppleHitFloor appleHitFloor;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        bounds = gridArea.bounds;
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        rb.velocity = Vector2.zero;

        // Pick a random position inside the bounds
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        // Round the values to ensure it aligns with the grid
        x = Mathf.Round(x);
        y = Mathf.Round(y);

        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RandomizePosition();

        if (other.CompareTag("Floor"))
            appleHitFloor?.Invoke();
    }
}
