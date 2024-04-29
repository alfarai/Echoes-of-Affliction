using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionSetter : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HandleHold()
    {
        
        switch (DataHub.ObjectInteracted.objectHeld)
        {

            
        }
    }
    
    public void HandleBreak()
    {
        switch (DataHub.ObjectInteracted.objectBroken)
        {
            case "PlankPlaceable":
                Debug.Log("A plank was placed!");
                break;
            case "Axe Large Wood":
                DataHub.ObjectiveHelper.hasBarrierBeenBroken = true;
               
                break;
        }
    }
    public void HandlePlace()
    {
        switch (DataHub.ObjectInteracted.objectPlacedOn)
        {
            case "PlaceableNeedsPlank":
                Debug.Log("A plank was placed!");
                break;
            case "LobbyDoorNeedsKeys":
                DataHub.ObjectiveHelper.hasKeyBeenPlaced = true;
                break;
            case "CarNeedsJerry":
                DataHub.ObjectiveHelper.hasFilledCarWithGas = true;
                break;

        }
        
    }
 /*   public void HandleEnterTrigger()
    {
        switch (DataHub.TriggerInteracted.lastTriggerEntered)
        {
            case "Spawn":
                //Debug.Log("Player is in Spawn");
                
                break;
            case "Lobby":
                DataHub.ObjectiveHelper.hasReachedLobby = true; 
                break;

        }
        
    }
    public void HandleExitTrigger()
    {
        switch (DataHub.TriggerInteracted.lastTriggerExited)
        {
            case "Spawn":
                Debug.Log("Player left Spawn");
                DataHub.ObjectiveHelper.hasExitedSpawn = true;
                break;

        }
    }*/
}
