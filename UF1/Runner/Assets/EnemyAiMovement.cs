using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiMovement : MonoBehaviour
{
    public float speed = 200;
    public float nextWaypointDistance = .75f;

    private int currentWaypoint;
    private Vector3 spawnReference;

    public enum Form { Triangle, Square, UpDown, LeftRight };
    public Form forma;
    private Vector3[] path;
    private Vector3[] trianglePath = {new Vector3(0, -2, 0), new Vector3(-3, 0, 0), new Vector3(0, 2, 0) };
    private Vector3[] squarePath = { new Vector3(0, -1, 0), new Vector3(-2, -1, 0), new Vector3(-2, 1, 0), new Vector3(0, 1, 0) };
    private Vector3[] upDownPath = { new Vector3(0, -1, 0), new Vector3(0, 1, 0) };
    private Vector3[] leftRightPath = { new Vector3(-2, 0, 0), new Vector3(2, 0, 0) };

    void Start() // inicializar variables
    {
        currentWaypoint = 0;
        switch (forma)
        {
            case Form.Triangle:
                path = trianglePath;
                break;
            case Form.Square:
                path = squarePath;
                break;
            case Form.UpDown:
                path = upDownPath;
                break;
            case Form.LeftRight:
                path = leftRightPath;
                break;
        }
        spawnReference = this.transform.position;
        InvokeRepeating("moveLeft", 0, .75f);
    }

    void FixedUpdate() // caluclo de movimiento
    {
        if (currentWaypoint >= path.Length)
        {
            currentWaypoint = 0;
        }

        Vector2 direction = (spawnReference + path[currentWaypoint] - this.transform.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        this.GetComponent<Rigidbody2D>().AddForce(force);

        float distance= Vector3.Distance(this.transform.position, spawnReference + path[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    private void moveLeft() // movimiento constante hacia la izquierda
    {
        spawnReference += new Vector3(-.3f, 0, 0);
    }

    private void OnDrawGizmosSelected() // gizmos debugging
    {
        if (path == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, nextWaypointDistance);
        for (int i = 0; i < path.Length; i++)
        {
            Gizmos.DrawSphere(spawnReference + path[i], 0.3f);
        }
        
    }
}
