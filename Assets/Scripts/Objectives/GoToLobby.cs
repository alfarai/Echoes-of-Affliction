using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLobby : IObjective
{
    
    public GameObject nextObjective;

    //complete conditions
    private bool hasReachedLobby;
    
    private string label = "GOAL 2: Go to the lobby.";

    void Awake()
    {
        DataHub.ObjectiveHelper.activeObjectiveID = GetGoalID();
    }
    void Update()
    {
       
    }
    
    public override void CallNextObjective()
    {
        
        Debug.Log("Goal 2 Completed");
        gameObject.SetActive(false);
        nextObjective.SetActive(true);
    }

    public override void DrawHUD()
    {
        GUILayout.Label(label);
    }

    public override int GetGoalID()
    {
        return 2;
    }

    public void SetHasReachedLobby()
    {
        hasReachedLobby = true;
    }

    public override void CompleteObjectiveCheck()
    {
        
        if (hasReachedLobby)
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
