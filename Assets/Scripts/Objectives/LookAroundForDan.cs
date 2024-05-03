﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LookAroundForDan : IObjective
{
    public GameObject nextObjective, goalLabel;
    private string label = "GOAL 7: Look around for Dan.";
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
        SetGoalText("Goal completed!");
        Invoke("CallNextObjective", 5f);
    }


    public override int GetObjectiveID()
    {
        return 7;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        if (DataHub.ObjectiveHelper.hasTalkedWithMarshall && DataHub.ObjectiveHelper.hasTalkedWithYoungBoy && !isComplete)
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
