using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GoToLobby : IObjective
{

    public GameObject nextObjective, goalLabel;



    private string label = "GOAL 2: Go to the lobby.";
    private bool isComplete;

   
  
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasReachedLobby && !isComplete)
        {
            CompleteObjective();
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            AutoFinish();
        }
    }


    public override void CallNextObjective()
    {

        
        gameObject.SetActive(false);
        nextObjective.SetActive(true);
    }

  

    public override int GetObjectiveID()
    {
        return 2;
    }

    public void SetHasReachedLobby()
    {
       //hasReachedLobby = true;
    }
    public override void AutoFinish()
    {
        CallNextObjective();
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
}
