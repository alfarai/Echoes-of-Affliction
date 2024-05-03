﻿using System.Collections;
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
    private DialogueText[] dialogue = new DialogueText[2];
    [SerializeField]
    private CinemachineVirtualCamera speakerCam;
    private bool isConversing, isOutlineActive;
    private Vector3 labelPos;
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
            DataHub.ObjectiveHelper.hasTalkedWithDan = true;
            DataHub.WorldEvents.hasFoundDan = true;
            
            
        }
        

    }
    private void StartConversation()
    {
        isConversing = true;

        //disable player movement
        player.SetIsAllowedMovement(false);

        //disable ui
        DataHub.PlayerStatus.isTalking = true;
        //set camera to speaker before initializing dialogue
        if (!dialogueController.GetIsInitialized())
        {
            speakerCam.enabled = true;
        }
        if (player.GetObjectHeld().Trim().Equals("Bandages"))
        {
            DataHub.ObjectiveHelper.hasGivenDanBandages = true;
            Talk(dialogue[1]);
            return;
        }
        Talk(dialogue[0]);
    }
    private void EndConversation()
    {

        if (gameObject.name == "Phone")
        {
            DataHub.ObjectiveHelper.hasFinishedCallingDan = true;
        }
        speakerCam.enabled = false;
        isConversing = false;
        //disable player movement
        player.SetIsAllowedMovement(true);
        DataHub.PlayerStatus.isTalking = false;
    }
    private void Talk(DialogueText dialogue)
    {
        dialogueController.ShowNextParagraph(dialogue);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        labelPos = transform.position;
        labelPos.y += 1.2f;
        labelPos.x += 1.2f;
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
                    Talk(dialogue[1]);
                }
                else
                {
                    Talk(dialogue[0]);
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
        clone = Instantiate(tooltipPrefab, labelPos, Quaternion.identity);
        clone.GetComponentInChildren<TextMesh>().text = "Talk";

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
