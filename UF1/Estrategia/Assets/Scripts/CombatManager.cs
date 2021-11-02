using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int healtPoints;
    public int damage;
    public bool isCastle;
    public bool isBuild;
    public bool isEnemy;
    public NomJugador data;

    public void GetHit(int damage)
    {
        StartCoroutine(Damaged());
        healtPoints -= damage;
        if (healtPoints <= 0)
        {
            if (isCastle) Destroy(gameObject);
            else if (isBuild) GetComponent<BuildControl>().DestroyBuild();
            else StartCoroutine(Die());
            if (isEnemy) data.punts++;
        }
    }

    IEnumerator Die()
    {
        GetComponent<Animator>().SetTrigger("die");
        GetComponent<AI>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    IEnumerator Damaged()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = c;
    }
}
