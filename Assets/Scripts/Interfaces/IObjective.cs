using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IObjective : MonoBehaviour
{
    
    public abstract int GetObjectiveID();
    
    public abstract void CompleteObjective();
    

    public abstract void CallNextObjective();
    public abstract void DrawHUD();
}

