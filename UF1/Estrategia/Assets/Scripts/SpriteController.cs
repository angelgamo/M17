using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{

    Animator animator;
    int direction;
    Vector2 movement;
    Vector2 last;

    public bool debug;

    private void Start()
    {
        animator = GetComponent<Animator>();
        last = transform.position;
        direction = 1;

        StartCoroutine(UpdateAll());
    }

    IEnumerator UpdateAll()
    {
        while (true)
        {
            movement = new Vector2(transform.position.x, transform.position.y) - last;

            if (movement.magnitude > 0.01)
            {
                direction = Direction(movement);
                animator.SetFloat("direction", direction);
            }

            animator.SetFloat("magnitude", movement.magnitude);

            last = transform.position;

            yield return new WaitForSeconds(.1f);
        }
    }

    private void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("hola");
                animator.SetTrigger("attack");
            }
        }
    }

    int Direction( Vector2 movement )
    {
        float angle = Vector2.SignedAngle(movement.normalized, Vector2.down);
        if (angle < 0) angle += 360;
        float stepCount = angle / 45;
        return Mathf.FloorToInt(stepCount) + 1;
    }
}
