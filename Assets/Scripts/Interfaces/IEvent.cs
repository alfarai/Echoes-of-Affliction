using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEvent : MonoBehaviour
{
    // Probability of triggering the action
    [Range(0f, 1f)]
    public float triggerProbability = 1f;

    // Abstract method to define the action to be performed when triggered
    protected abstract void PerformAction();

    // Abstract method to define the conditions for triggering the action
    protected abstract bool ShouldTrigger(Collider other);

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone meets the conditions
        // Here, you can check for specific tags or layers if needed
        if (ShouldTrigger(other))
        {
            // Trigger the action based on probability
            if (Random.value < triggerProbability)
            {
                // Perform the action
                PerformAction();

               
            }

            Invoke("DestroyObject", 5f);
        }
        
    }
    private void DestroyObject()
    {
        // Destroy the object since the trigger should happen only once
        Destroy(gameObject);
    }

}

