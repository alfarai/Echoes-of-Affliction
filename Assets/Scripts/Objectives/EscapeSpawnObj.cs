using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSpawnObj : IObjective
{
    
    public GameObject nextObjective;
    
    //compete conditions
    private bool hasExitedSpawn;

    private string label = "GOAL 1: Escape the living room.";
    void Awake()
    {
        DataHub.ObjectiveHelper.activeObjectiveID = GetGoalID();
    }
    void Update()
    {
        
    }
    

    public override int GetGoalID()
    {
        return 1;
    }

    public void SetHasExitedSpawn()
    {
        hasExitedSpawn = true;
    }
    public override void  CompleteObjectiveCheck()
    {
       
        if (hasExitedSpawn)
        {
            label = "Goal completed!";
            Invoke("CallNextObjective", 5f);
            
        }
        
    }
    

    public override void DrawHUD()
    {
        GUILayout.Label(label);
    }
    public override void CallNextObjective()
    {
        //update ui
        label = "Goal completed!";


        //make next objective active
        gameObject.SetActive(false);
        nextObjective.SetActive(true);
    }

    void OnGUI()
    {
        DrawHUD();
    }
    
}
