﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FindBandagesForDan : IObjective
{
    public float timer = 300.0f;
    public GameObject timerObj;
    private TextMeshProUGUI timerText;
    private float totalMinutes;
    private int wholeMinute, seconds;
    public GameObject nextObjective, goalLabel, cutscene;
    private string label = "GOAL 11: Find bandages for Dan.";
    private bool isComplete;
    public override void AutoFinish()
    {
        CallNextObjective();
    }

    public void CallNextObjectiveOnFail()
    {

        cutscene.SetActive(true); //isCutscenePlaying is set to true here. Set to false after execution
        if (!DataHub.PlayerStatus.isCutscenePlaying)
        {
            gameObject.SetActive(false);
            nextObjective.SetActive(true);
        }
    }
    public override void CallNextObjective()
    {
        gameObject.SetActive(false);
        nextObjective.SetActive(true);
    }

    public override void CompleteObjective()
    {
        isComplete = true;
        SetGoalText("Goal Completed!");
        Invoke("CallNextObjective", 5f);
    }
    private void ObjectiveFailed()
    {
        isComplete = true;
        SetGoalText("Goal Failed!");
        Invoke("CallNextObjectiveOnFail", 5f);
    }
    
    

    public override int GetObjectiveID()
    {
        return 11;
    }

    // Start is called before the first frame update
    void Start()
    {
        //offset by 1 sec to display initial value properly
        timer += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasGivenDanBandages && !isComplete)
        {
            timerObj.SetActive(false);
            CompleteObjective();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            AutoFinish();
        }
        if (!isComplete && DataHub.ObjectiveHelper.hasFinishedTalkingWithDan)
        {
            timer -= Time.deltaTime;
            totalMinutes = timer / 60.0f;
            wholeMinute = (int)totalMinutes;
            seconds = (int)((totalMinutes - wholeMinute) * 60);

            if (seconds.ToString().Length == 1)
            {
                timerText.text = wholeMinute + ":0" + seconds;
            }
            else
            {
                timerText.text = wholeMinute + ":" + seconds;
            }
        }
        
        
        
        if (timer <= 0 && !isComplete && !DataHub.ObjectiveHelper.hasGivenDanBandages)
        {
            DataHub.ObjectiveHelper.hasFailedToGiveDanBandages = true;
            timerObj.SetActive(false);
            ObjectiveFailed();
        }


    }
    void Awake()
    {
        timerText = timerObj.GetComponent<TextMeshProUGUI>();
        SetGoalText(label);
    }

    public override void SetGoalText(string label)
    {
        goalLabel.GetComponent<TextMeshProUGUI>().text = label;
    }
}
