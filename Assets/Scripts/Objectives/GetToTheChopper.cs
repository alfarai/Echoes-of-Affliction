using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetToTheChopper : IObjective
{
    private AudioManager audio;
    public GameObject nextObjective, goalLabel;
    private string label = "GOAL 12: Get to the chopper!";
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
        return 12;
    }

    // Start is called before the first frame update
    void Start()
    {
        flash = GameObject.Find("FlashMessager").GetComponent<FlashInfo>();
        audio.PlayMusic(audio.bg2);
        StartCoroutine(flash.FlashMessage("The rescue chopper is at the outskirts of the city.", 5));
    }

    // Update is called once per frame
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasReachedChopper && !isComplete)
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
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    public override void SetGoalText(string label)
    {
        goalLabel.GetComponent<TextMeshProUGUI>().text = label;
    }
}
