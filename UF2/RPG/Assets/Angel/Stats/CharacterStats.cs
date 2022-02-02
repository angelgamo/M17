using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Invecible Frames")]
    [SerializeField] float IFTtime = .3f;
    bool isIVF;
    public List<Item> items;

    public bool isWeaponMagic = false;

    [Header("Stats")]
    public Stat Strength;       // physical damage
    public Stat Agility;        // attack speed
    public Stat Intelligence;   // magic damage
    public Stat PhysicResist;   // physical damage resistance
    public Stat MagicResist;    // magical damage resistance
    public Stat MoveSpeed;      // character speed

    [Header("Resources")]
    public Resource Health;

    [SerializeField] GameEvent die;

    private void Start()
    {
        Health.ModifyValue(Health.MaxValue.Value);
    }

	public virtual void RecieveDamage(float damage, AttackType attackType)
    {
        if (isIVF)
            return;

        if (attackType == AttackType.Physical)
            damage -= PhysicResist.Value;
        else if (attackType == AttackType.Magic)
            damage -= MagicResist.Value;

        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        if (damage > 0)
            StartCoroutine(IVFrames());

        // PopUP damage

        Health.ModifyValue(-damage);
        if (Health.Value <= 0)
            Die();
    }

    IEnumerator IVFrames()
    {
        isIVF = true;
        yield return new WaitForSeconds(IFTtime);
        isIVF = false;
    }

    public virtual void RestoreHealth(float health)
    {
        Health.ModifyValue(health);
        if (Health.Value <= 0)
            Die();
    }

    public virtual void Die()
    {
        // Spawn random btw(0, level) coins
        SpawnManager.istance.SpawnCoin(true, Random.Range(1, 3 + (int)Mathf.Sqrt(LevelsSystem.instance.level)), transform.position);

        // Spawn items
        if (items.Count > 0)
            foreach (var item in items)
                if (Random.Range(1, 4)==3)
                    SpawnManager.istance.SpawnObject(item, false, 1, transform.position);

        GameObject.Find("Spawn").GetComponent<EnemySpawner>().EnemyDie();
        StartCoroutine(DelayDie());
    }

    IEnumerator DelayDie()
	{
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}

public enum AttackType
{
    Physical,
    Magic
}