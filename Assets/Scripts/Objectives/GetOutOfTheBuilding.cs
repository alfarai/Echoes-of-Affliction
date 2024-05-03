using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GetOutOfTheBuilding : IObjective
{

    public GameObject nextObjective, goalLabel;
    public GameObject door;

    private bool isComplete;

    private string label = "GOAL 3: Get out of the building";

 
    void Update()
    {
        //Debug.Log(DataHub.ObjectInteracted.interactedObj.name);
        if (DataHub.ObjectiveHelper.hasKeyBeenPlaced && DataHub.ObjectiveHelper.hasBarrierBeenBroken && !isComplete)
        {
            CompleteObjective();
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            AutoFinish();
        }
    }
    public override void AutoFinish()
    {
        CallNextObjective();
    }
    public override void CallNextObjective()
    {
        


        gameObject.SetActive(false);
        nextObjective.SetActive(true);

        

        
    }

    

    public override int GetObjectiveID()
    {
        return 3;
    }
    public void SetHasKeyBeenPlaced()
    {
        //check if objects labelled with tag contains the required object for completion and see if player interacted with it (if it is saved in DataHub, it was recently interacted by the player 
        if (GameObject.FindGameObjectsWithTag("Objective3").Contains(DataHub.ObjectInteracted.interactedObj))
        {
            //hasKeyBeenPlaced = true;
        }

    }
    public void SetHasBarrierBeenBroken()
    {
        if (GameObject.FindGameObjectsWithTag("Objective3").Contains(DataHub.ObjectInteracted.interactedObj))
        {
            //hasBarrierBeenBroken = true;
        }

    }
    public override void CompleteObjective()
    {

        isComplete = true;
        SetGoalText("Goal completed!");
        Invoke("CallNextObjective", 5f);




    }


    void Awake()
    {
        SetGoalText(label);
    }
    public override void SetGoalText(string label)
    {
        goalLabel.GetComponent<TextMeshProUGUI>().text = label;
    }
    public void OpenDoor()
    {
        door.transform.rotation = Quaternion.Euler(0, -90f, 0);
    }
}
