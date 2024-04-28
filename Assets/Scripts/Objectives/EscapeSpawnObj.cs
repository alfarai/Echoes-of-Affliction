using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EscapeSpawnObj : IObjective
{

    public GameObject nextObjective;

    
    private string label = "GOAL 1: Escape the living room.";
    private bool isComplete;







    void Awake()
    {
        DataHub.ObjectiveHelper.activeObjectiveID = GetObjectiveID();
        SetGoalText(label);

    }
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasExitedSpawn && !isComplete)
        {
            CompleteObjective();
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            AutoFinish();
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
    public override void AutoFinish()
    {
        CallNextObjective();
    }

    public override void SetGoalText(string label)
    {
        GameObject.Find("Objective Text").GetComponent<TextMeshProUGUI>().text = label;
    }
    public override void CallNextObjective()
    {
        //update ui
        label = "Goal completed!";


        //make next objective active
        gameObject.SetActive(false);
        nextObjective.SetActive(true);
    }

    

}
