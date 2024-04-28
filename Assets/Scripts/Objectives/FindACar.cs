using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FindACar : IObjective
{
    public GameObject nextObjective;
    private string label = "GOAL 4: Find a car.";
    private bool isComplete;
    public override void AutoFinish()
    {
        CallNextObjective();
    }

    public override void CallNextObjective()
    {

        gameObject.SetActive(false);
        nextObjective.SetActive(true);
    }

    public override void CompleteObjective()
    {
        isComplete = true;
        label = "Goal completed!";
        Invoke("CallNextObjective", 5f);
    }

    

    public override int GetObjectiveID()
    {
        return 4;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasFoundACar && !isComplete)
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
        GameObject.Find("Objective Text").GetComponent<TextMeshProUGUI>().text = label;
    }
}
