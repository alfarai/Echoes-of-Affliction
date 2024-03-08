using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLobby : IObjective
{
    public GameObject playerObj;
    public GameObject nextObjective;
    private Character player;
    private bool isComplete = false, isGoalStatusShown = false;
    private string label = "GOAL 2: Go to the lobby.";

    void Awake()
    {
        player = playerObj.GetComponent<Character>();

    }
    public override void CallNextObjective()
    {
        Debug.Log("Goal 2 Completed");
        gameObject.SetActive(false);
        //nextObjective.SetActive(true);
    }

    public override void DrawHUD()
    {
        GUILayout.Label(label);
    }

    public override int GetGoalID()
    {
        return 2;
    }

    public override bool isGoalComplete()
    {
        isComplete = player.GetHasReachedLobby();
        return isComplete;
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
