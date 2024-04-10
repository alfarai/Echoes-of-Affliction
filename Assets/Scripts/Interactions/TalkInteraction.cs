using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TalkInteraction : MonoBehaviour, IInteractable
{
    //public GameObject playerObj;
    
    private Character player;
    public void Interact()
    {
        Debug.Log("Talking");
        
        //show dialogue flow with player

        if(gameObject.name == "Marshall")
        {
            DataHub.ObjectiveHelper.hasTalkedWithMarshall = true;
        } 
        if(gameObject.name == "Young Boy")
        {
            DataHub.ObjectiveHelper.hasTalkedWithYoungBoy = true;
        }

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
