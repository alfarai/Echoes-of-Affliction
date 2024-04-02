using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GetOutOfTheBuilding : IObjective
{

    public GameObject nextObjective;


    private bool isComplete;

    private string label = "GOAL 3: Get out of the building";

    void Awake()
    {
        DataHub.ObjectiveHelper.activeObjectiveID = GetObjectiveID();
        //DataHub.ObjectiveHelper.conditionCount = conditionCount;
    }
    void Update()
    {
        //Debug.Log(DataHub.ObjectInteracted.interactedObj.name);
        if (DataHub.ObjectiveHelper.hasKeyBeenPlaced && DataHub.ObjectiveHelper.hasBarrierBeenBroken && !isComplete)
        {
            CompleteObjective();
        }
    }

    public override void CallNextObjective()
    {
        Debug.Log("Loading Level 2");
        /*gameObject.SetActive(false);
        nextObjective.SetActive(true);*/

        //load next level
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public override void DrawHUD()
    {
        GUILayout.Label(label);
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
        label = "Goal completed!";
        Invoke("CallNextObjective", 5f);




    }


    void OnGUI()
    {
        DrawHUD();
    }
}
