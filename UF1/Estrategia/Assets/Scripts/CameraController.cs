using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 Origin;
    public int mouseButton;

    void Update()
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return;
        }

        if (!Input.GetMouseButton(mouseButton)) return;

        Vector3 newPos = Origin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position += newPos;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -16, 15), Mathf.Clamp(transform.position.y, -8, 12), transform.position.z);
        Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
