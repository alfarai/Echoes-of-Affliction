using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSpawnObj : IObjective
{

    public GameObject nextObjective;

    
    private string label = "GOAL 1: Escape the living room.";
    private bool isComplete;







    void Awake()
    {
        DataHub.ObjectiveHelper.activeObjectiveID = GetObjectiveID();

    }
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasExitedSpawn && !isComplete)
        {
            CompleteObjective();
        }
    }


    public override int GetObjectiveID()
    {
        return 1;
    }

    public void SetHasExitedSpawn()
    {
        //hasExitedSpawn = true;
    }
    public override void CompleteObjective()
    {

        isComplete = true;
        label = "Goal completed!";
        Invoke("CallNextObjective", 5f);



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
