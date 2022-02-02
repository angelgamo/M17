using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target; //Empty transform placed as child on player at offset

    Vector3 velocity = Vector3.zero;
    [SerializeField] Vector2 limits;

    float smoothTime = 0.4f;

    void LateUpdate()
    {
        if (Mathf.Abs(target.position.x) > limits.x || Mathf.Abs(target.position.y) > limits.y)
        {
            target.position = new Vector2(Mathf.Clamp(target.position.x, -limits.x, limits.x), Mathf.Clamp(target.position.y, -limits.y, limits.y));
        }
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
