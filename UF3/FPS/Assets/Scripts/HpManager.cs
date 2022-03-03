using System.Collections;
using UnityEngine;

public class HpManager : MonoBehaviour
{
    public float hpMax;
    public float hp;
    public GameEvent die;

    private void Start()
    {
        hp = hpMax;
    }

    public virtual void ReceiveDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0f)
        {
            hp = 0f;
            Die();
        }
        else
        {
            StopCoroutine("RestoreHealth");
            StartCoroutine("RestoreHealth");
        }
    }

    RaycastHit hitInfo;

    protected virtual void Die()
    {
        Vector3 randomPos = new Vector3(Random.value * .77f, 0f, Random.value * .77f);
        randomPos *= Random.Range(-40f, 40f);
        randomPos.y = 150f;

        Physics.Raycast(randomPos, Vector3.down, out hitInfo, 200f);
        randomPos.y = hitInfo.point.y + 1f;

        hp = hpMax;

        transform.position = randomPos;
        die.Raise();
    }

    IEnumerator RestoreHealth()
    {
        yield return new WaitForSeconds(2f);
        while (hp < hpMax)
        {
            hp++;
            yield return new WaitForSeconds(.1f);
        }
    }
}
