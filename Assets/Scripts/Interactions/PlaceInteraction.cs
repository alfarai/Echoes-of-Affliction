using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class PlaceInteraction : MonoBehaviour, IInteractable
{
    public bool doesThisConsume;
    public UnityEvent PlaceEvent;
    private Character player;
    public int maxPlaceableCount; //0 if it this does not consume
    private int placeCount;
    public GameObject tooltipPrefab;
    private GameObject clone;
    private Vector3 labelPos;
    private bool isOutlineActive;

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
            
            if (doesThisConsume)
            {
                
                ++placeCount;
                
                Debug.Log("Placed " + placeCount + "/" + maxPlaceableCount);
                //remove object held
                player.DropObjectHeld(true);

                DataHub.ObjectInteracted.objectPlacedOn = gameObject.name;
                DestroyLabel();
                if (placeCount == maxPlaceableCount)
                {
                    //invoke place event
                    PlaceEvent.Invoke();
                }
                return;
            }
            //place object held to template by setting position
            GameObject obj = player.GetGameObjectHeld();
            //Collider objCol = obj.GetComponent<Collider>();
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
            }
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.GetComponent<Rigidbody>().isKinematic = true;
            

            //remove object held
            player.DropObjectHeld(true);

            DataHub.ObjectInteracted.objectPlacedOn = gameObject.name;
            DestroyLabel();
            //invoke place event
            PlaceEvent.Invoke();
            return;
            
        }
        else
        {
            Debug.Log("This doesn't belong here...");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        labelPos = transform.position;
        labelPos.y -= 1.1f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    
    public void InstantiateLabel()
    {
        clone = Instantiate(tooltipPrefab, labelPos, Quaternion.identity);
        clone.GetComponentInChildren<TextMesh>().text = "Place";

    }
    public void DestroyLabel()
    {
        Destroy(clone);

    }
    public void EnableOutline(bool flag)
    {
        //if already active, dont enable
        if (isOutlineActive && flag)
            return;
        isOutlineActive = flag;
        gameObject.GetComponent<Outline>().enabled = flag;
    }
}
