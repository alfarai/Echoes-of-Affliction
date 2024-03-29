﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    private Vector3 pos = new Vector3(0, 0, 0);
    public GameObject cameraObj;
    private Camera cam;
    private Character player;
    private RaycastHit hitInfo;


    // Start is called before the first frame update
    void Start()
    {
        cam = cameraObj.GetComponent<Camera>();
        player = GetComponent<Character>();

    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = cam.ScreenPointToRay(pos);
        //Debug.DrawRay(ray.origin, ray.direction * InteractRange,Color.green);
        Debug.DrawRay(transform.position, cam.transform.forward * InteractRange, Color.green);
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            //if player is holding an object and is on top of a placeable interaction, cast ray down to ensure interaction.
            //in place interaction
            if (player.GetIsPlayerOnPlaceable())
            {
                Ray r = new Ray(transform.position, -Vector3.up * InteractRange);



                if (Physics.Raycast(r, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                    {
                        interactObj.Interact();

                    }
                }
            }
            //dont forget to put ignore raycast on checkpoint colliders so it doesnt interact with interactables overlapping checkpoints
            //if raycast hit an interactable object
            else if (Physics.Raycast(new Ray(transform.position, cam.transform.forward * InteractRange), out hitInfo) & hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                //Debug.Log("Interacting...");
                interactObj.Interact();

            }
            
        }
        


    }
}

