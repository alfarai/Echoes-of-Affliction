using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IObjective : MonoBehaviour
{
    public abstract int GetGoalID();
    public abstract bool isGoalComplete();

    public abstract void CallNextObjective();
    public abstract void DrawHUD();
}

