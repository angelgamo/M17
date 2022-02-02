using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine2 : MonoBehaviour
{
    [Header("State Machine")]
    [SerializeField] State2[] states;

    void OnValidate()
    {
        states = GetComponentsInChildren<State2>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("0")) Pop();
        else if (Input.GetKeyDown("1")) SetState(0);
        else if (Input.GetKeyDown("2")) SetState(1);
    }

    void Pop()
    {
        foreach (var state in states) state.enabled = false;
    }

    void SetState(int i)
    {
        foreach (var state in states) state.enabled = false;
        states[i].enabled = true;
    }
}
