using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed = 2f;

    private void Update()
    {
        float interpolation = speed * Time.deltaTime;
        Vector3 position = transform.position;

        position.x = Mathf.Lerp(transform.position.x, target.position.x, interpolation);
        position.y = Mathf.Lerp(transform.position.y, target.position.y, interpolation);


        transform.position = position;
    }
}
