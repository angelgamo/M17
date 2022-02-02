using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnimation : State2
{
    public override void Init()
    {
        GetComponent<Animator>().StartPlayback();
    }

    public override void Exit()
    {
        GetComponent<Animator>().StopPlayback();
    }
}
