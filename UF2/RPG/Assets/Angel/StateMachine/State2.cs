using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State2 : MonoBehaviour
{
    public abstract void Init();

    public abstract void Exit();

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        Exit();
    }
}
