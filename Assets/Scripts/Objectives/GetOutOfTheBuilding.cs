using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GetOutOfTheBuilding : IObjective
{
    
    public GameObject nextObjective;
    

    //complete conditions
    //requires interaction with Large Wood and KeyPlaceable Lobby Door
    private bool hasKeyBeenPlaced, hasBarrierBeenBroken;
    private int conditionCount = 2;

    private string label = "GOAL 3: Get out of the building";

    void Awake()
    {
        DataHub.ObjectiveHelper.activeObjectiveID = GetGoalID();
        DataHub.ObjectiveHelper.conditionCount = conditionCount;
    }
    void Update()
    {
        //Debug.Log(DataHub.ObjectInteracted.interactedObj.name);
    }
  
    public override void CallNextObjective()
    {
        Debug.Log("Goal 3 Completed");
        gameObject.SetActive(false);
        nextObjective.SetActive(true);
    }

    public override void DrawHUD()
    {
        GUILayout.Label(label);
    }

    public override int GetGoalID()
    {
        return 3;
    }
    public void SetHasKeyBeenPlaced()
    {
        //check if objects labelled with tag contains the required object for completion and see if player interacted with it (if it is saved in DataHub, it was recently interacted by the player 
        if(GameObject.FindGameObjectsWithTag("Objective3").Contains(DataHub.ObjectInteracted.interactedObj))
        {
            hasKeyBeenPlaced = true;
        }
        
    }
    public void SetHasBarrierBeenBroken()
    {
        if(GameObject.FindGameObjectsWithTag("Objective3").Contains(DataHub.ObjectInteracted.interactedObj))
        {
            hasBarrierBeenBroken = true;
        }
        
    }
    public override void CompleteObjectiveCheck()
    {
        
        if(DataHub.ObjectiveHelper.conditionCount == 0)
        {
            label = "Goal completed!";
            Invoke("CallNextObjective", 5f);
            
        }
        
        
    }


    void OnGUI()
    {
        DrawHUD();
    }
}
