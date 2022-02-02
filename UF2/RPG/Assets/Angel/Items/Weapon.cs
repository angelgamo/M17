using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Objects/Weapon")]
public class Weapon : Equipment
{
    public Vector3 AttackOffset;
    public float AttackSpeed;
}
