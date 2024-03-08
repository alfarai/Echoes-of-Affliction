using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSpawnObj : IObjective
{
    public GameObject playerObj;
    private Character player;
    private string label = "Goal 1";
    void Awake()
    {
        player = playerObj.GetComponent<Character>();
       
    }
    
    public override int GetGoalID()
    {
        return 1;
    }

    public override bool isGoalComplete()
    {
        return player.GetHasLeftSpawn();
    }

    public override void DrawHUD()
    {
        GUILayout.Label(label);
    }
    
    void OnGUI()
    {
        DrawHUD();
        if (isGoalComplete())
        {
            label = "Goal complete";
        }
    }
}
