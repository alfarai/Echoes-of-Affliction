using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLobby : IObjective
{

    public GameObject nextObjective;

    

    private string label = "GOAL 2: Go to the lobby.";
    private bool isComplete;

    void Awake()
    {
        DataHub.ObjectiveHelper.activeObjectiveID = GetObjectiveID();

    }
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasReachedLobby && !isComplete)
        {
            CompleteObjective();
        }
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

    public override int GetObjectiveID()
    {
        return 2;
    }

    public void SetHasReachedLobby()
    {
       //hasReachedLobby = true;
    }

    public override void CompleteObjective()
    {

        isComplete = true;
        label = "Goal completed!";
        Invoke("CallNextObjective", 5f);




    }

    void OnGUI()
    {
        DrawHUD();
    }

}
