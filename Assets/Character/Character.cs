using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Character : MonoBehaviour
{

    private StaminaManager staminaManager;
    public Vector3 objectPosition;
    public bool isOnTeleportZone = false;
    public static event Action onJump;
    private Animator animator;
    private Vector2 input;
    private CharacterController characterController;

    [SerializeField] private float speed = 5f, walkingSpeed = 5f, runSpeed = 3f;

    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;
    private Vector3 move;


    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float verticalVelocity;

    private Vector3 moveDir;


    [SerializeField] private float jumpPower = 3f;
    private bool hasJumped, hasLanded, isFalling, isRunning, isInVehicle, isCrouching, isMoving, isSitting, isBoosted, isAbleToTeleport;

    private float speedTemp;




    private float hp;


    //public GameObject hpObj;
    //private UpdateHP hpScript;
    public Transform cam;
    public GameObject cameraObj;
    private Camera camera;
    private RaycastHit hit;

    private Vector3 place;
    private bool hasClickedTPLocation;

    private bool hasLeftSpawn = false, hasReachedLobby = false;

    private void Awake()
    {

    }


    void Start()
    {
        //hpScript = hpObj.GetComponent<UpdateHP>();
        //hp = hpScript.GetHP(); //set hp
        speedTemp = speed;
        //animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        camera = cameraObj.GetComponent<Camera>();

        staminaManager = GetComponent<StaminaManager>();
        objectPosition = transform.position;
        //boss = skeletonBossObj.GetComponent<SkeletonBoss>();
    }
    private void LateUpdate()
    {

    }
    // Update is called once per frame
    private void Update()
    {

        ApplyGravity();
        ApplyRotationAndMovement();
        if (staminaManager.staminaBar.value == 0)
        {
            speed = walkingSpeed;
        }

        /*
        bloodParticles.transform.position = transform.position;
        if (hp != hpScript.GetHP()) //compare old hp initialized from Start() to current hp, if it changed, emit blood
        {
            hp = hpScript.GetHP(); //reset hp to current
            bloodParticles.SetActive(true);
            Invoke("DisableBlood", 1.5f);
        }

        if (hasClickedTPLocation) //is player has clicked on a spot to tp, force player position to go at new place. not doing this doesn't tp the player for some reason so just force it.
        {
            if((transform.position - place).magnitude > 1)
            {
                transform.position = place;
            }
            else
            {
                
                hasClickedTPLocation = false;
            }
        }
        if (isAbleToTeleport)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            teleportParticles.transform.position = transform.position;
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("LMB Pressed!");
                Vector3 v = transform.position;
                v.y -= 1f; //play particles on feet
                teleportEffect.transform.position = v;
                teleportEffect.SetActive(true);


                ray = camera.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y,camera.nearClipPlane));

                if (Physics.Raycast(ray, out hit))
                {
                    Invoke("TeleportToPos", 2f);
 
                    
                }
            }
            tpTimeTemp -= Time.deltaTime;
            if (tpTimeTemp < 0)
            {
                isAbleToTeleport = false;
                teleportParticles.SetActive(false);
                tpTimeTemp = teleportTime;
                //Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
                
        }
        

        if (isBoosted)
        {
            speedBoostParticles.transform.position = transform.position;
            if(boostTimeTemp > 0)
            {
                boostTimeTemp -= Time.deltaTime;
            }
            else
            {
                boostTimeTemp = boostTime;
                isBoosted = false;
                RemoveBoost();
            }
            
        }
        */
        if (Input.GetKey(KeyCode.Q) && isOnTeleportZone)
        {
            climbLocation();
        }
            if (Input.GetKeyDown(KeyCode.C))
        {


            if (!isCrouching)
            {
                Debug.Log("Crouching...");
                //animator.SetBool("isCrouching", true);
                isCrouching = true;
                if (isCrouching && isMoving)
                {
                    Debug.Log("Crouch Walking...");
                    //animator.SetBool("isCrouchWalking", true);
                    speed = speed / 2;
                }

            }
            else if (isCrouching)
            {
                Debug.Log("Not Crouching");
                //animator.SetBool("isCrouching", false);
                isCrouching = false;
            }


        }
        /* if (Input.GetKeyDown(KeyCode.F))
         {
             if (!isSitting)
             {
                 Debug.Log("Entering Vehicle...");
                 //animator.SetBool("isEnteringVehicle", true);
                 isSitting = true;
             }
             else
             {
                 Debug.Log("Exiting Vehicle");
                 //animator.SetBool("isEnteringVehicle", false);
                 isSitting = false;
             }

         }
         if(isCrouching && isMoving)
         {
             Debug.Log("Crouch Walking...");
             animator.SetBool("isCrouchWalking", true);
         }
         else if(isCrouching && !isMoving)
         {
             Debug.Log("Not Crouch Walking");
             animator.SetBool("isCrouchWalking", false);
         }*/


        //check if player is falling
        if (!characterController.isGrounded && !hasJumped)
        {
            hasJumped = false;
            isFalling = true;
            hasLanded = false;
            //animator.SetBool("isJumping", hasJumped);
            //animator.SetBool("isGrounded", hasLanded);
            //animator.SetBool("isFalling", isFalling);
        }
        //check if player landed
        if (characterController.isGrounded && isFalling)
        {

            hasLanded = true;
            isFalling = false;
            //animator.SetBool("isGrounded", hasLanded);
            //animator.SetBool("isFalling", isFalling);
            // Debug.Log("Landed");
            hasLanded = false;
        }

        //check if player jumped
        if (hasJumped)
        {
            if (characterController.isGrounded)
            {
                hasJumped = false;
                hasLanded = true;
                isFalling = false;
                //animator.SetBool("isJumping", hasJumped);
                //animator.SetBool("isGrounded", hasLanded);
                //animator.SetBool("isFalling", isFalling);
                //Debug.Log("Landed");

            }
            else
            {
                isFalling = true;
                //animator.SetBool("isFalling", isFalling);
                // Debug.Log("Falling...");
            }
        }





    }

    private void OnEnable()
    {


    }

    private void OnDisable()
    {

    }



    
    public void climbLocation()
    {
        Debug.Log("hel");
        transform.position = objectPosition;
      
    }







    private void ApplyRotationAndMovement()
    {
        //movement from this tutorial: https://www.youtube.com/watch?v=4HpC--2iowE and the a good boy games channel
        if (input.sqrMagnitude == 0)
        {
            return;
        }


        //face direction of movement
        var targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //find angle require to move to get to point (x,y). cam.eulerangles.y takes into account cam rotation
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime); //smooth angle so it wont snap
        transform.rotation = Quaternion.Euler(0f, angle, 0f); //apply rotation

        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // turn rotation into diretion by multiplying vector3.forward. by having cam.eulerangles.y, we move into the direction of cam rotation
        moveDir.y = verticalVelocity;
        //movement
        characterController.Move(moveDir * Time.deltaTime * speed);


    }
    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -1.0f;
        }
        else
        {
            verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        moveDir = Vector3.zero;
        moveDir.y = verticalVelocity;
        characterController.Move(moveDir * Time.deltaTime * speed);
        //reset moveDir so that it only applies gravity

    }


    //this method is called in playerinput object events
    public void Move(InputAction.CallbackContext context)
    {



        if (context.canceled && !context.performed)
        {
            //       Debug.Log("WASD released");

            //animator.SetBool("isWalking", false);
            isMoving = false;
            if (isCrouching && !isMoving)
            {
                //    Debug.Log("Not Crouch Walking");
                //animator.SetBool("isCrouchWalking", false);
                speed = speedTemp;
            }
        }
        if (context.started && !context.performed)
        {
            //    Debug.Log("WASD pressed...");
            //animator.SetBool("isWalking", true);
            isMoving = true;


        }

        input = context.ReadValue<Vector2>();
        //  Debug.Log(input);
        move = new Vector3(input.x, 0, input.y);
        //move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    }
    public void Jump(InputAction.CallbackContext context)
    {
        //https://forum.unity.com/threads/learning-new-inputsystem-when-and-how-is-the-cancelled-callback-used.969501/

        if (context.performed && !hasJumped)
        {
            //  Debug.Log("Jump pressed");
            hasJumped = true;
            hasLanded = false;
            isFalling = true;
            //animator.SetBool("isJumping", hasJumped);
            //animator.SetBool("isGrounded", hasLanded);
            verticalVelocity += jumpPower;
        }


        /*if (context.performed) && hasJumped)
        {
            Debug.Log("In air...");
            //animator.SetBool("isJumping", false);
        }
        if ((!context.started && characterController.isGrounded) || hasJumped)
        {
            Debug.Log("Landed");
            hasJumped = false;
            //animator.SetBool("isJumping", false);
        }*/




    }
    public void Run(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Player is Running...");
            isRunning = true;
            isMoving = true;
            //animator.SetBool("isRunning", isRunning);

            speed += runSpeed; //isRunning is true if shift is held



            staminaManager.RegenStamina(false);

        }
        if (context.canceled && !context.performed)
        {
            Debug.Log("Player isn't Running...");
            isRunning = false;
            isMoving = false;
            //animator.SetBool("isRunning", isRunning);
            speed -= runSpeed;
            staminaManager.RegenStamina(true);
        }

        //move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SpawnCP")
        {
            Debug.Log("Player entered Spawn");
            
        }
        if (other.tag == "LobbyCP")
        {
            Debug.Log("Player entered Lobby");
            hasReachedLobby = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SpawnCP")
        {
            Debug.Log("Player exited Spawn");
            hasLeftSpawn = true;
        }
        if (other.tag == "LobbyCP")
        {
            Debug.Log("Player exited Lobby");

        }

    }
    public bool GetHasLeftSpawn()
    {
        return hasLeftSpawn;
    }
    public bool GetHasReachedLobby()
    {
        return hasReachedLobby;
    }
}
