using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//https://www.youtube.com/watch?v=J01z1F-du-E&ab_channel=GameDevBeginner 
//https://www.youtube.com/watch?v=W91QkppPpHI&ab_channel=RootGames
public class GameEventListener : MonoBehaviour
{
    //game event to listen to
    public GameEvent Event;

    //response if game event is raised
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.AddListener(this);
    }
    private void OnDisable()
    {
        Event.RemoveListener(this);
    }
    public void OnEventRaised()
    {
        //Debug.Log("Event Raised!");
        Response.Invoke();
    }


}
