﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceInteraction : MonoBehaviour, IInteractable
{
    public GameObject playerObj,template;
    public bool doesThisConsume;
    public UnityEvent PlaceEvent;
    private Character player;
    public int maxPlaceableCount; //0 if it this does not consume
    private int placeCount;

    public void Interact()
    {
        
        //if player is holding on empty object (not holding anything)
        if (GameObject.Find(player.GetObjectHeld()) == GameObject.Find("EmptyObj"))
        {
            Debug.Log("You need to find something to proceed!");
            return;
        }
        //to place an object, the string value of objectHeld must be a substring of the targeted PlaceInteraction object
        //ex: if placeable name is "KeyPlaceable" and player pressed e while holding Plank, player can't place. Otherwise, if it is PlankPlaceable, player can place plank
        //if what player is holding matches the place interaction
        if (gameObject.name.Contains(player.GetObjectHeld()))
        {
            Debug.Log(placeCount);
            if (doesThisConsume)
            {
                
                ++placeCount;
                
                Debug.Log("Placed " + placeCount + "/" + maxPlaceableCount);
                //remove object held
                player.DropObjectHeld(true);

                DataHub.ObjectInteracted.objectPlacedOn = gameObject.name;



                if (placeCount == maxPlaceableCount)
                {
                    //invoke place event
                    PlaceEvent.Invoke();
                }

                
                return;
            }
            //place object held to template by setting position
            //GameObject obj = player.itemsArrayObj.GetComponent<ItemsArray>().itemGameObjects.Find(x => x.name == player.GetObjectHeld());
            GameObject obj = player.GetGameObjectHeld();
            Collider objCol = obj.GetComponent<Collider>();
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
            }
            obj.transform.position = template.transform.position;
            obj.transform.rotation = template.transform.rotation;
            objCol.attachedRigidbody.useGravity = true;

            //remove object held
            player.DropObjectHeld(true);


            //set datahub values
            /*DataHub.ObjectInteracted.interactedObj = gameObject;
            DataHub.ObjectInteracted.interactable = player.GetObjectHeld();
            DataHub.ObjectInteracted.interaction = "place";*/

            DataHub.ObjectInteracted.objectPlacedOn = gameObject.name;
            
            //invoke place event
            PlaceEvent.Invoke();
            return;
            
        }
        //add else here for if it doesnt match
        else
        {
            Debug.Log("This doesn't belong here...");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
