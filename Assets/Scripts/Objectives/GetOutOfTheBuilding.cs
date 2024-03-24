using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutOfTheBuilding : IObjective
{
    
    public GameObject nextObjective;
    

    //complete conditions
    private bool hasKeyBeenPlaced, hasBarrierBeenBroken;

    private string label = "GOAL 3: Get out of the building";

    void Awake()
    {
        

    }
    void Update()
    {
        
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
        //check if KeyPlaceable interaction was successfully interacted with. 
        if(DataHub.ObjectInteracted.interactedObj.name == "KeyPlaceable")
        {
            hasKeyBeenPlaced = true;
        }
        
    }
    public void SetHasBarrierBeenBroken()
    {
        hasBarrierBeenBroken = true;
    }
    public override void CompleteObjectiveCheck()
    {
        
        if(hasKeyBeenPlaced && hasBarrierBeenBroken)
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
