using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakInteraction : MonoBehaviour, IInteractable
{
    public GameObject playerObj;
    public int hitsBeforeBreaking = 0;
    private Character player;
    public void Interact()
    {
        //if player is holding on empty object (not holding anything)
        if (GameObject.Find(player.GetObjectHeld()) == GameObject.Find("EmptyObj"))
        {
            Debug.Log("You need to find something to proceed!");
            return;
        }
        //to break an object, the string value of objectHeld must be a substring of the targeted BreakInteraction object
        //ex: if breakable name is "HammerBreakable" and player pressed e while holding Plank, player can't break. Otherwise, if it is Hammer, player can break object named HammerBreakable
        //match breakable interaction with breakable object
        if (gameObject.name.Contains(player.GetObjectHeld()))
        {
            --hitsBeforeBreaking;
            //break object if 0
            if(hitsBeforeBreaking == 0)
            {
                gameObject.SetActive(false);
                Debug.Log("Object broken!");
            }
            else if (hitsBeforeBreaking > 0)
            {
                Debug.Log("Object hit! " + "(" + hitsBeforeBreaking + " left)");
            }

            
        }
        //add else here for if it doesnt match
        else
        {
            Debug.Log("You can't break this with a " + player.GetObjectHeld());
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
