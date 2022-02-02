using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterCombat))]
[RequireComponent(typeof(EnemyEquipmentManager))]
public class EnemyIA : MonoBehaviour
{
    public float speed;
    public Transform enemy;
    public List<State> states;
    public State currentState;
    public Transform playerPos;
    public float range;
    public float visionRange;
    public float attackDistance;
    public float attackMargin;
    bool isFlipped;
    public bool isMelee;
    float lastPos;
    [SerializeField] bool debug = false;

    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        currentState = states[0];
        currentState.init();
        this.GetComponent<Animator>().SetBool("isRunning",true);
        speed = this.GetComponent<CharacterStats>().MoveSpeed.Value;
    }

    public void UpdateStats()
	{
        speed = this.GetComponent<CharacterStats>().MoveSpeed.Value;
    }

    void Update()
    {
        currentState.update();
        FlipCharacter();
    }

    void FlipCharacter()
    {
        Vector3 flipped = transform.localScale;
        flipped.x *= -1f;
        float velocity = transform.position.x - lastPos;

        //Debug.Log(velocity);

        if (velocity < 0 && !isFlipped && (velocity < -0.01f || velocity > 0.01f))
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
        else if (velocity >= 0 && isFlipped && (velocity < -0.01f || velocity > 0.01f))
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        lastPos = transform.position.x;
    }

    public void changeState(State s)
    {
        currentState.exit();
        currentState = s;
        currentState.init();
    }

    private void OnDrawGizmos()
    {
        if (!debug)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance + attackMargin);

        if (isMelee)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackDistance - attackMargin);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackDistance - attackMargin / 3);
        Gizmos.DrawWireSphere(transform.position, attackDistance + attackMargin / 3);

        
    }
}
