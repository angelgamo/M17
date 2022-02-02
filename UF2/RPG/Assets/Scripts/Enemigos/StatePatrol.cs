using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(menuName = "ScriptableObject/State/patrol")]
public class StatePatrol : State
{
    EnemyIA ia;
    public List<Vector3> path;
    Vector3 currentPath;
    int direction;
    int currentPos = 0;
    public bool comeBack;
    Rigidbody2D rb;

    public void Start()
    {
        ia = gameObject.GetComponent<EnemyIA>();
        for (int i = 0; i < path.Count; i++)
        {
            path[i] += transform.position;
        }
        currentPath = path[currentPos];
        direction = 1;
        rb = this.GetComponent<Rigidbody2D>();
    }
    public override void exit()
    {
        
    }

    public override void init()
    {
        
    }

    public override void update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, currentPath, ia.speed * Time.deltaTime);
        rb.MovePosition(Vector2.MoveTowards(transform.position, currentPath, ia.speed * Time.deltaTime));
        if (Vector2.Distance(transform.position, currentPath) < 0.1f)
        {
            currentPos += direction;
            if (comeBack)
            {
                if (currentPos + direction >= path.Count) direction = -1;
                else if (currentPos + direction < 0) direction = 1;
            }
            else
            {
                if (currentPos + direction >= path.Count) currentPos = -1;
            }
            currentPath = path[currentPos + direction];
        }
        if (Vector2.Distance(transform.position, ia.playerPos.position) < ia.visionRange)
        {
            ia.changeState(ia.states[1]);
        }
    }
}
