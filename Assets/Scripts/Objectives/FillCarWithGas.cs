using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillCarWithGas : IObjective
{
    public GameObject nextObjective;
    private string label = "GOAL 5: Fill the car with gas.";
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

    public override void DrawHUD()
    {
        GUILayout.Label(label);
    }

    public override int GetObjectiveID()
    {
        return 5;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DataHub.ObjectiveHelper.hasFilledCarWithGas && !isComplete)
        {
            CompleteObjective();
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            AutoFinish();
        }
    }
    void OnGUI()
    {
        DrawHUD();
    }
}
