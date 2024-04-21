using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using TMPro;
public class TalkInteraction : MonoBehaviour, IInteractable
{
    //public GameObject playerObj;
    [SerializeField]
    private DialogueController dialogueController;
    private Character player;
    [SerializeField]
    private DialogueText dialogue;
    [SerializeField]
    private DialogueText dialogue2;
    [SerializeField]
    private CinemachineVirtualCamera speakerCam;
    private bool isConversing,flag;

    public GameObject tooltipPrefab;
    private GameObject clone;

    public void Interact() 
    {
        StartConversation();
        

 

        if(gameObject.name == "Marshall")
        {
            DataHub.ObjectiveHelper.hasTalkedWithMarshall = true;
        } 
        if(gameObject.name == "Young Boy")
        {
            DataHub.ObjectiveHelper.hasTalkedWithYoungBoy = true;
        }
        if (gameObject.name == "Dan")
        {
            DataHub.WorldEvents.hasFoundDan = true;
            
            
        }

    }
    private void StartConversation()
    {
        isConversing = true;

        //disable player movement
        player.SetIsAllowedMovement(false);

        //set camera to speaker before initializing dialogue
        if (!dialogueController.GetIsInitialized())
        {
            speakerCam.enabled = true;
        }
        if (player.GetObjectHeld().Trim().Equals("Bandages"))
        {
            DataHub.ObjectiveHelper.hasGivenDanBandages = true;
            Talk(dialogue2);
            return;
        }
        Talk(dialogue);
    }
    private void EndConversation()
    {
        speakerCam.enabled = false;
        isConversing = false;
        //disable player movement
        player.SetIsAllowedMovement(true);
    }
    private void Talk(DialogueText dialogue)
    {
        dialogueController.ShowNextParagraph(dialogue);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isConversing)
        {
            DestroyLabel();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (player.GetObjectHeld().Trim().Equals("Bandages"))
                {
                    Talk(dialogue2);
                }
                else
                {
                    Talk(dialogue);
                }
                
                //if dialogue controller finished showing text
                if (dialogueController.GetHasEndedConversation())
                {
                    EndConversation();
                    InstantiateLabel();
                }
            }
        }
        
    }

    public void InstantiateLabel()
    {
        clone = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
        clone.GetComponentInChildren<TextMesh>().text = gameObject.name;

    }
    public void DestroyLabel()
    {
        Destroy(clone);
    }
}
