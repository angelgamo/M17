using UnityEngine;

public class StatePatrol2 : State
{
    float angle;
    Vector2 startPont;
    [SerializeField, Range(.1f, 10f)] float distance;
    [SerializeField, Range(.1f, 10f)] float speed;
    float velocity;
    [Space]
    [SerializeField, Range(0f, 5f)] float variationForce = 1f;
    [SerializeField, Range(0f, 5f)] float directionVariation = 0.1f;
    [SerializeField, Range(.1f, 1f)] float returnForce = .1f; 
    [Space]
    [SerializeField] bool debug = false;
    [SerializeField] EnemyIA ia;

	private void Awake()
	{
        ia = gameObject.GetComponent<EnemyIA>();
    }

	public override void init()
    {
        startPont = transform.position;
        float random = Random.Range(0, 360);
    }

    public override void update()
    {
        angle += variationForce * (Mathf.PerlinNoise(Time.time * directionVariation, 0.0f) - .5f);
        velocity = Mathf.Clamp01(speed * Mathf.PerlinNoise(0.0f, Time.time * directionVariation) - .2f);
        Move();
        OutOfBounds();
        angle = (angle + 360) % 360;

        if (Vector2.Distance(transform.position, ia.playerPos.position) < ia.visionRange)
            ia.changeState(ia.states[1]);
    }

    public override void exit()
    {
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position + AngleToVector(angle) * 2, velocity * Time.deltaTime);
    }

    void OutOfBounds()
    {
        if (Vector2.Distance(transform.position, startPont) > distance)
        {
            float angleToCenter = GetAngle(transform.position, startPont);
            float delta = (360 + (angle - angleToCenter)) % 360;
            if (delta < 180 && delta > 5)
                angle -= returnForce;
            else
                angle += returnForce;
        }
    }

    Vector3 AngleToVector(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
    }

    float GetAngle(Vector2 point1, Vector2 point2)
    {
        Vector2 delta = point1 - point2;
        return ((Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg) + 180);
    }

    private void OnDrawGizmos()
    {
        if (!debug)
            return;

        Gizmos.color = Color.yellow;
        Vector2 angleVector = transform.position + AngleToVector(angle);
        Vector2 up = AngleToVector(angle + 210) * .3f;
        Vector2 down = AngleToVector(angle + 150) * .3f;
        Gizmos.DrawLine(transform.position, angleVector);
        Gizmos.DrawLine(angleVector, angleVector + up);
        Gizmos.DrawLine(angleVector, angleVector + down);
        Gizmos.DrawSphere(startPont, .1f);
        Gizmos.DrawWireSphere(startPont, distance);
    }
}
