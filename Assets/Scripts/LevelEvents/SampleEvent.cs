using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEvent : IEvent
{
    protected override void PerformAction()
    {
        Debug.Log("Event triggered! Performing action...");
    }

    protected override bool ShouldTrigger(Collider other)
    {
        return other.CompareTag("Player");
    }
}
