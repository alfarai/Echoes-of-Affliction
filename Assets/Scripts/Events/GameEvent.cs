using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void AddListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
    public void Raise()
    {
        for(int i = listeners.Count-1; i>= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }
}
//Example case of how the event system works
/* Example:
 * In the Character.cs, we detect if player exits or enters a collider. If the player exited spawn collider, an ExitSpawn.Invoke event call is made. In the inspector, an exitspawnevent scriptable object is attached,
 * which contains a Raise() method, which tells the listeners attached to the event that an event has occured. each listeners, OnEventRaised() method is called, which invokes the Response.Invoke() call, which
 * calls attached functions/gameobjects/scripts in the inspector where the listener script is attached, to perform task and react to that event
 */
 