using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GoToToweringTwins : IObjective
{
    public GameObject nextObjective, goalLabel;
    private string label = "GOAL 10: Go to the Towering Twins to save Dan!";
    private bool isComplete;
    private FlashInfo flash;
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
        return 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(flash.FlashMessage("The Towering Twins is identified by two identical buildings next to each other.", 5));

    }

    // Update is called once per frame
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasReachedToweringTwins && DataHub.ObjectiveHelper.hasTalkedWithDan && !isComplete)
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
        flash = GameObject.Find("FlashMessager").GetComponent<FlashInfo>();
    }
    public override void SetGoalText(string label)
    {
        goalLabel.GetComponent<TextMeshProUGUI>().text = label;
    }
}
