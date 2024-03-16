using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInteraction : MonoBehaviour, IInteractable
{
    public GameObject playerObj,template;
    private Character player;

    public void Interact()
    {
        if (GameObject.Find(player.GetObjectHeld()) == GameObject.FindWithTag("EmptyObj"))
        {
            Debug.Log("You need to find something to proceed!");
            return;
        }
        //to place an object, the name of object held by player must be part of the string in the tag of this placeable interaction obj
        //ex: player is holding plank, the player then interacted with the gap whose tag is PlankCP, therefore we can do a place interaction here
        if (gameObject.tag.Contains(player.GetObjectHeld()))
        {
            

            //place object held to template by setting position
            Debug.Log("Placed Object");
            GameObject obj = GameObject.Find(player.GetObjectHeld());
            obj.transform.position = template.transform.position;
            obj.transform.rotation = template.transform.rotation;

            //remove object held
            player.SetIsHoldingObj(false);
            player.SetObjectHeld(GameObject.FindWithTag("EmptyObj").name);
            return;
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
