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
        /*if (Input.GetKeyDown(KeyCode.E) && player.GetIsHoldingObj())
        {
            player.SetIsHoldingObj(false);
            player.SetObjectHeld(GameObject.FindWithTag("EmptyObj"));

        }*/
        if (Input.GetKeyDown(KeyCode.E))
        {

            Ray r = new Ray(transform.position, cam.transform.forward * InteractRange);
            RaycastHit hitInfo;


            if (Physics.Raycast(r, out hitInfo))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                    
                }
            }

        }
        
    }
}

