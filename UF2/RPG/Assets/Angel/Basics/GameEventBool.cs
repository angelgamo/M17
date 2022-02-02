// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameEventBool", menuName = "GameEvents/GameEventBool")]
public class GameEventBool : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    protected readonly List<GameEventListenerBool> eventListeners =
        new List<GameEventListenerBool>();

    public void Raise(bool value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(value);
    }

    public void RegisterListener(GameEventListenerBool listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerBool listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
