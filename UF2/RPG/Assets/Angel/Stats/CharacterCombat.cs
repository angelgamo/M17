using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    CharacterStats myStats;

    void Awake()
    {
        myStats = GetComponent<CharacterStats>();
    }
    
    public void Attack(CharacterStats targetStats)
    {
        if (myStats.isWeaponMagic)
            targetStats.RecieveDamage(myStats.Intelligence.Value, AttackType.Magic);
        else
            targetStats.RecieveDamage(myStats.Strength.Value, AttackType.Physical);
    }
}
