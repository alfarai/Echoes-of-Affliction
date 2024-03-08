using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSpawnObj : IObjective
{
    public GameObject playerObj;
    public GameObject nextObjective;
    private Character player;
    private bool isComplete = false, isGoalStatusShown = false;
    private string label = "GOAL 1: Escape the living room.";
    void Awake()
    {
        player = playerObj.GetComponent<Character>();

    }

    public override int GetGoalID()
    {
        return 1;
    }

    public override bool isGoalComplete()
    {
        isComplete = player.GetHasLeftSpawn();
        return isComplete;
    }

    public override void DrawHUD()
    {
        GUILayout.Label(label);
    }
    public override void CallNextObjective()
    {
        Debug.Log("Goal 1 Completed");
        gameObject.SetActive(false);
        nextObjective.SetActive(true);
    }

    void OnGUI()
    {
        
        DrawHUD();

        if (!isGoalStatusShown && isGoalComplete())
        {
            label = "Goal completed!";
            isGoalStatusShown = true;
            Invoke("CallNextObjective", 5f);
        }


        

    }
    
}
