using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FindAPhone : IObjective
{
    public GameObject nextObjective, goalLabel;
    private string label = "GOAL 8: Find a telephone booth to call Dan.";
    private bool isComplete;
    public override void AutoFinish()
    {
        CallNextObjective();
    }

    public override void CallNextObjective()
    {
        //call aftershock 2
        gameObject.SetActive(false);
        nextObjective.SetActive(true);
    }

    public override void CompleteObjective()
    {
        isComplete = true;
        SetGoalText("Goal completed!");
        Invoke("CallNextObjective", 5f);
    }

    

    public override int GetObjectiveID()
    {
        return 8;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasFoundTelephone && !isComplete && DataHub.ObjectiveHelper.hasFinishedCallingDan)
        {

            CompleteObjective();
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            AutoFinish();
        }
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
