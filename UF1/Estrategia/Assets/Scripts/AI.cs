using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float speed = 1;
    public float attackRange = 1;
    public float visionRange = 5;

    public List<Transform> enemiesInVision;
    public List<Transform> enemiesInAttack;
    public List<AI> alliesInVision;

    public bool ally;
    public bool attack;
    public bool target;
    public Vector3 targetPosition;

    public string enemyTag;

    int damage;
    public bool canAttack;
    public float timeAttack;

    void Start()
    {
        alliesInVision = new List<AI>();
        attack = false;
        target = false;
        ally = false;
        canAttack = true;
        damage = GetComponent<CombatManager>().damage;

        InicializeChilds();

        StartCoroutine("CheckEnemy");
    }

    private void FixedUpdate()
    {
        if (target && !attack || ally && !attack)
        {
            Vector3 movement = targetPosition - transform.position;
            transform.position += movement.normalized * speed * Time.deltaTime;
        }
    }

    private void InicializeChilds()
    {
        GameObject childAttack = new GameObject("Attack");
        childAttack.AddComponent<Attack>(); 

        GameObject childVision = new GameObject("Vision");
        childVision.AddComponent<Vision>();

        childAttack.transform.position = transform.position + new Vector3(0, .2f, 0);
        childVision.transform.position = transform.position + new Vector3(0, .2f, 0);

        childAttack.layer = 0;
        childVision.layer = 0;

        childAttack.transform.parent = transform;
        childVision.transform.parent = transform;
    }

    IEnumerator CheckEnemy()
    {
        while (true)
        {
            if (enemiesInAttack.Count > 0) // If enemies in range Attack
            {
                attack = true;
                target = false;
                ally = false;

                // attack
                if (canAttack)
                {
                    try
                    {
                        List<Transform> enemyList = new List<Transform>(enemiesInAttack);

                        foreach (Transform enemy in enemyList)
                        {
                            GetComponent<Animator>().SetTrigger("attack");
                            StartCoroutine(Attack(enemy));
                            continue;
                        }
                    }
                    catch { }
                }
                yield return new WaitForSeconds(0.1f);
            }
            else if (enemiesInVision.Count > 0) // else if enemies in range Vision
            {
                attack = false;
                target = true;
                ally = false;
                ClosestEnemy();
                yield return new WaitForSeconds(0.1f);
            }
            else if (alliesInVision.Count > 0) // else if enemies in allies range Vision
            {
                attack = false;
                target = false;
                ally = false;
                try
                {
                    List<AI> alliesList = new List<AI>(alliesInVision);

                    foreach (AI ally in alliesList)
                    {
                        if (ally.attack || ally.target)
                        {
                            this.ally = true;
                            targetPosition = ally.targetPosition;
                        }
                    }
                }
                catch { }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                attack = false;
                target = false;
                ally = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ClosestEnemy()
    {
        targetPosition = enemiesInVision[0].position;
        foreach (Transform enemy in enemiesInVision)
        {
            if (Vector3.Distance(transform.position, enemy.position) < Vector3.Distance(transform.position, targetPosition))
            {
                targetPosition = enemy.position;
            }
        }
    }

    IEnumerator Attack(Transform enemy)
    {
        canAttack = false;
        yield return new WaitForSeconds(0.8f);
        if (enemy != null) enemy.GetComponent<CombatManager>().GetHit(damage);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, .2f, 0), attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, .2f, 0), visionRange);

        if (target)
        {
            Gizmos.color = new Color(1, 0, 1, 1);
            Gizmos.DrawLine(transform.position, targetPosition);
        }
        else if (ally)
        {
            Gizmos.color = new Color(1, 1, 0, 1);
            Gizmos.DrawLine(transform.position, targetPosition);
        }
    }
}
