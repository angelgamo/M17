using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float zoom;
    public int min = 1;
    public int max = 7;
    public float speed = .1f;

    private void Start()
    {
        zoom = Camera.main.orthographicSize;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom > min)
        {
            zoom -= zoom*speed;
            zoom = Mathf.Clamp(zoom, min, max);
            Camera.main.orthographicSize = zoom;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom < max)
        {
            zoom += zoom*speed;
            zoom = Mathf.Clamp(zoom, min, max);
            Camera.main.orthographicSize = zoom;
        }

    }
}
