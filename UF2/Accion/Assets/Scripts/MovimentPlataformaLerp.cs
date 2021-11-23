using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentPlataformaLerp : MonoBehaviour
{
    
    bool goToA;
    bool finished;
    public int currentWaypoint = 0;
    float nextWaypointDistance = .75f;
    public float speed;
    public Vector3[] wayPoints;

    // Start is called before the first frame update
    void Start()
    {
        goToA = true;
        finished = false;
        //StartCoroutine(LerpPositionAmbSlowDown(wayPoints[0], 3));
    }

    void FixedUpdate() // calculo de movimiento
    {
        if (currentWaypoint >= wayPoints.Length)
        {
            currentWaypoint = 0;
        }

        Vector2 direction = (wayPoints[currentWaypoint] - this.transform.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        float distance = Vector3.Distance(this.transform.position, wayPoints[currentWaypoint]);

        this.transform.position = Vector2.MoveTowards(this.transform.position, wayPoints[currentWaypoint], speed * Time.deltaTime);


        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (finished)
        {
            finished = false;
            if (goToA)
            {
                //StartCoroutine(LerpPositionAmbSlowDown(wayPoints[0], 3));
            }
            else
            {
                //StartCoroutine(LerpPositionAmbSlowDown(wayPoints[1], 3));
            }
        }
    }

    IEnumerator LerpPositionAmbSlowDown(Vector3 target, float duration)
    {
        Debug.Log("Començant Corrutina");
        float time = 0;
        Vector2 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        finished = true;
        goToA = !goToA;
        transform.position = target;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 point in wayPoints)
        {
            Gizmos.DrawSphere(point, 0.2f);
        }
    }
}
