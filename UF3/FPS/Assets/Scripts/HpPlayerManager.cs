using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HpPlayerManager : HpManager
{
	public Image healthBar;

    public void ReceiveHealth(float health)
	{
        hp += health;
        if (hp > hpMax)
            hp = hpMax;
        UpdateHealthBar();
    }

	public override void ReceiveDamage(float damage)
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

        UpdateHealthBar();
	}

	protected override void Die()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void UpdateHealthBar()
	{
		healthBar.fillAmount = hp / hpMax;
	}

    IEnumerator RestoreHealth()
    {
        yield return new WaitForSeconds(2f);
        while (hp < hpMax)
        {
            hp++;
            UpdateHealthBar();
            yield return new WaitForSeconds(.1f);
        }
    }
}
