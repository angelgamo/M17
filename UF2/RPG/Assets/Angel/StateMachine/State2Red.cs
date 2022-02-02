using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State2Red : State2
{
    public override void Init()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public override void Exit()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
