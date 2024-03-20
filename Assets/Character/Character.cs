using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Character : MonoBehaviour
{

    private StaminaManager staminaManager;
    private CharacterController characterController;
    private Inventory inv;
    private ItemsArray itemsArray;
    private Vector3 place;
    private bool hasClickedTPLocation;
    private Camera camera;
    private RaycastHit hit;
    private Animator animator;

    private Vector2 input;
    private Vector3 move;
    private Vector3 moveDir;



    private float gravity = -9.81f;

    private float verticalVelocity;
    private float currentVelocity;
    private bool hasJumped, hasLanded, isFalling, isRunning, isInVehicle, isCrouching, isMoving, isSitting, isBoosted, isAbleToTeleport;
    private bool hasLeftSpawn = false, hasReachedLobby = false, isHoldingObj = false, isPlayerOnPlaceable = false;
    private float speedTemp;

    public Vector3 objectPosition;
    public string objectHeld = "EmptyObj";
    public bool isOnTeleportZone = false;
    public static event Action onJump;

    public Transform cam;
    public GameObject cameraObj;
    public GameObject itemsArrayObj;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 5f, walkingSpeed = 5f, runSpeed = 3f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float jumpPower = 3f;

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
        inv = GetComponent<Inventory>();
        itemsArray = itemsArrayObj.GetComponent<ItemsArray>();
        staminaManager = GetComponent<StaminaManager>();
        objectPosition = transform.position;

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
        if (Input.mouseScrollDelta.y > 0)
        {
            //Debug.Log("scrolling up");
            inv.MovePointerBackward();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            //Debug.Log("scrolling down");
            inv.MovePointerForward();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropObjectHeld(false);
        }

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
        if (other.tag == "PlankCP")
        {
            isPlayerOnPlaceable = true;

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
        if (other.tag == "PlankCP")
        {
            isPlayerOnPlaceable = false;

        }

    }
    public bool GetIsPlayerOnPlaceable()
    {
        return isPlayerOnPlaceable;
    }
    public bool GetHasLeftSpawn()
    {
        return hasLeftSpawn;
    }
    public bool GetHasReachedLobby()
    {
        return hasReachedLobby;
    }

    public bool GetIsHoldingObj()
    {
        return isHoldingObj;
    }
    /*
     When player clicks e, for hold interactions, we grab the gameobject's name in the editor, then pass it to HoldObject().
     Ensure that we have a non empty object before adding to inventory. If it is an empty object, we are not holding anything, then return
        - EmptyObj occurs when focused inventory slot is empty

     */
    public void HoldObject(string objName)
    {
        if (objName == "EmptyObj")
        {
            isHoldingObj = false;
            return;
        }
        else
        {
            isHoldingObj = true;
        }

        //add to inventory
        inv.Add(itemsArray.interactables.Find(item => item.name == objName));

        //make object disappear/inactive
        GameObject obj = itemsArray.itemGameObjects.Find(x => x.name == objName);
        obj.SetActive(false);

    }
    /* When dropping an object, we get the item on focused inventory slot. If it is empty, we don't have any items to drop
     * otherwise, remove it from our inventory
     */
    public void DropObjectHeld(bool isPlaced)
    {
        objectHeld = GetObjectHeld();
        if (objectHeld == "EmptyObj")
        {
            Debug.Log("No items to drop!");
            return;
        }
        //remove from inventory       
        inv.Remove(itemsArray.interactables.Find(item => item.name == objectHeld));


        if (!isPlaced)
        {
            //make object reappear beside player
            GameObject obj = itemsArray.itemGameObjects.Find(x => x.name == objectHeld);
            obj.SetActive(true);

            obj.transform.position = gameObject.transform.position;
            obj.transform.rotation = gameObject.transform.rotation;

            Debug.Log("Dropped " + objectHeld);
        }
        else
        {
            Debug.Log("Placed " + objectHeld);
        }

    }
    //retrieves item on focused inventory slot
    public string GetObjectHeld()
    {
        return inv.GetActiveItem();
    }
    /*public void SetObjectHeld()
    {

        objectHeld = inv.GetActiveItem();
        Debug.Log("Player is holding a " + objectHeld);

    }*/
}
/*
 Interactables that can be put in inventory must have an item data under scripts>inventory>items
 These item data must then be put in the hierarchy under Player>inventory interactables then drag and drop the item data into the array. This serves as the item data database, which lists props
    that can be placed into inventory.

 For placeable interactions, the naming convention must be [name of game object that matches placeable]Placeable.
    For example, we have a PlankPlaceable interaction in the first level and we need a Plank for it. The naming must be consistent. If we have ShortPlankPlaceable, we need ShortPlank for it.

 */