using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRotate : MonoBehaviour
{
    [SerializeField] Weapon2 hand;
    [SerializeField] PlayerController pc;
    
    public void ActiveRotate()
	{
        hand.rotate = true;
        pc.lookAtMouse = true;
	}

    public void DeactiveRotate()
	{
        hand.rotate = false;
        pc.lookAtMouse = false;
	}
}
