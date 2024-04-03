using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEvent : IEvent
{
    public GameObject fire;
    protected override void PerformAction()
    {
        fire.SetActive(true);
    }

    protected override bool ShouldTrigger(Collider other)
    {
        return other.CompareTag("Player");
    }

    
}
