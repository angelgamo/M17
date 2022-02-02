// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerBool : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEventBool Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<bool> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(bool value)
    {
        Response.Invoke(value);
    }
}
