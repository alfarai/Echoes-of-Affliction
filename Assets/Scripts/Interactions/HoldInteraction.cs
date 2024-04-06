using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HoldInteraction : MonoBehaviour, IInteractable
{
    //public GameObject playerObj;
    public UnityEvent HoldEvent;
    private Character player;
    public void Interact()
    {
        Collider objCol = GetComponent<Collider>();
        objCol.attachedRigidbody.useGravity = true;
        player.HoldObject(gameObject.name,gameObject);

        /*DataHub.ObjectInteracted.interactedObj = gameObject;
        DataHub.ObjectInteracted.interactable = player.GetObjectHeld();
        DataHub.ObjectInteracted.interaction = "hold";*/

        DataHub.ObjectInteracted.objectHeld = player.GetObjectHeld();
        HoldEvent.Invoke();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
