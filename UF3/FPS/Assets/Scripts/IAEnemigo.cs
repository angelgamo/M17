using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class IAEnemigo : MonoBehaviour
{
    NavMeshAgent agent;
    Weapon weapon;

    public int currentWaypoint = 0;
    public List<Vector3> wayPoints;
    public GameObject player;
    Transform target;
    public bool following = false;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
        weapon = GetComponentInChildren<Weapon>();
	}

	void Start()
    {
        target = player.transform;
    }

    void FixedUpdate()
    {
        if (EnemyFOV() && !following)
        {
            activarFollow(player.transform);
        }

        if (following)
        {
            agent.destination = target.position;
            if (agent.remainingDistance < agent.stoppingDistance)
                transform.LookAt(agent.destination);
            Shoot();
            ISiElDeixoEnPau();
        }
        else
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                currentWaypoint++;
                if (currentWaypoint >= wayPoints.Count)
                    currentWaypoint = 0;

                agent.destination = wayPoints[currentWaypoint];
            }
        }
    }

    bool EnemyFOV()
    {
        float maxDistance = 30;
        float anguloFov = 45;

        Debug.DrawRay(this.transform.position, this.transform.forward * 10, Color.magenta, 0.75f, false);

        if (Vector3.Distance(this.transform.position, player.transform.position) < maxDistance)
        {
            if(Vector3.Angle(this.transform.forward, player.transform.position - transform.position) < anguloFov && Vector3.Angle(this.transform.forward, player.transform.position - transform.position) > anguloFov * -1)
            {
                NavMeshHit nmh;

                if(!agent.Raycast(player.transform.position, out nmh))
                    return true;
            }
        }
        return false;
    }

    Ray ray;
    RaycastHit hitInfo;

    void Shoot()
	{
        ray.origin = transform.position + transform.forward;
        ray.direction = target.position - transform.position;

        if (Physics.Raycast(ray, out hitInfo, 100f))
            if (hitInfo.collider.tag == "Player")
                weapon.StartFiring();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 point in wayPoints)
            Gizmos.DrawSphere(point, 0.2f);
    }

    public void activarFollow(Transform tar)
    {
        this.following = true;
        target = tar;
    }

    public void perderVista()
    {
        this.following = false;
    }

    public void ISiElDeixoEnPau()
    {
        float lostDistance = 70;
        if (Vector3.Distance(this.transform.position, player.transform.position) > lostDistance)
            perderVista();
    }

    [Button("CreateWayPoint")]
    public void CreateWayPoint()
	{
        wayPoints.Add(transform.position);
	}
}
