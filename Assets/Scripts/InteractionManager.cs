using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
public class InteractionManager : MonoBehaviour
{
    //public UnityEvent completeObjectiveEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HandleInteractionEvent()
    {
        GameObject interactedObj = DataHub.ObjectInteracted.interactedObj;
        string interactable = DataHub.ObjectInteracted.interactable;
        string interaction = DataHub.ObjectInteracted.interaction; //hold, place, break, breakloot
        if (interaction.Equals("hold"))
        {
            if (!DataHub.PlayerStatus.isInventoryFull)
            {
                Debug.Log("Player held " + interactable);
            }
            
        }
        if (interaction.Equals("place"))
        {
            Debug.Log("Player placed " + interactable + " on " + interactedObj);
            
        }
        if (interaction.Equals("break"))
        {
            Debug.Log("Player broke " + interactedObj + " with " + interactable);
        }
    }
}
