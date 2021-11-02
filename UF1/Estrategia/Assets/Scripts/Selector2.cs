using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector2 : MonoBehaviour
{
    public delegate void SetTarget(Transform target);
    public event SetTarget onSetTarget;
    public GameObject target;
    GameObject targetClone;


    private LineRenderer lineRend;
    private Vector2 initialMousePosition, currentMousePosition;
    private BoxCollider2D boxColl;

    void Start()
    {
        targetClone = Instantiate(target);
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 0;
        gameObject.tag = "Seleccionador";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            lineRend.positionCount = 4;
            lineRend.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
            lineRend.SetPosition(1, new Vector2(initialMousePosition.x, initialMousePosition.y));
            lineRend.SetPosition(2, new Vector2(initialMousePosition.x, initialMousePosition.y));
            lineRend.SetPosition(3, new Vector2(initialMousePosition.x, initialMousePosition.y));

            boxColl = gameObject.AddComponent<BoxCollider2D>();
            boxColl.isTrigger = true;
            boxColl.offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        if (Input.GetMouseButton(0))
        {
            currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            lineRend.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
            lineRend.SetPosition(1, new Vector2(initialMousePosition.x, currentMousePosition.y));
            lineRend.SetPosition(2, new Vector2(currentMousePosition.x, currentMousePosition.y));
            lineRend.SetPosition(3, new Vector2(currentMousePosition.x, initialMousePosition.y));

            transform.position = (currentMousePosition + initialMousePosition) / 2;

            boxColl.size = new Vector2(
                Mathf.Abs(initialMousePosition.x - currentMousePosition.x),
                Mathf.Abs(initialMousePosition.y - currentMousePosition.y));
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRend.positionCount = 0;
            Destroy(boxColl);
            transform.position = Vector3.zero;
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (onSetTarget != null)
            {
                targetClone.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject go = new GameObject();
                Destroy(go, 10);
                go.transform.position = targetClone.transform.position;
                go.transform.rotation = targetClone.transform.rotation;
                try
                {
                    onSetTarget.Invoke(go.transform);
                }
                catch
                {

                }
            }
        }
    }
}

