using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;


public class Character : MonoBehaviour
{
    private AudioManager audio;
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

    private GameObject teleportTrigger;

    public List<GameObject> objectsHeld = new List<GameObject>();


    private float gravity = -9.81f;

    private float verticalVelocity;
    private float currentVelocity;
    private bool hasJumped, hasLanded, isFalling, isRunning, isCrouching, isMoving, isSitting, isBoosted, isAbleToTeleport, isAllowedMovement = true;
    private bool isPlayerOnPlaceable = false;
    private float speedTemp;

    //public Vector3 objectPosition;
    public string objectHeld = "EmptyObj";
    public bool isOnTeleportZone, isTeleporting,isNearbyVehicle,isInVehicle,isExitingVehicle;
    public static event Action onJump;

    private Transform cameraTransform;
    public GameObject ThirdPersonCamera;
    public GameObject itemsArrayObj;
    public float pushPower = 2.0f;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 5f, walkingSpeed = 5f, runSpeed = 3f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float jumpPower = 3f;

    public UnityEvent ExitTrigger, EnterTrigger, TakeDamageEvent;
    private GameObject car;
    private float elapsed;
    public List<AudioClip> feet;
    private AudioSource playerAudioSource;

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
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraTransform = camera.transform;
        inv = GetComponent<Inventory>();
        itemsArray = itemsArrayObj.GetComponent<ItemsArray>();
        staminaManager = GetComponent<StaminaManager>();
        //objectPosition = transform.position;
        car = GameObject.Find("Car");
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        playerAudioSource = gameObject.GetComponent<AudioSource>();
    }
   
    private void FixedUpdate()
    {
        
        if (isOnTeleportZone && isTeleporting)
        {
            transform.position = teleportTrigger.transform.position;
            isTeleporting = false;
            
        }
        if (isExitingVehicle)
        {
            GameObject car = GameObject.Find("Car");
            transform.position = new Vector3(car.transform.position.x + 5, car.transform.position.y, car.transform.position.z);
            transform.rotation = car.transform.rotation;
            if(transform.position.x >= car.transform.position.x)
            {
                isExitingVehicle = false;
            }
            
            
        }
        
    }
    // Update is called once per frame
    private void Update()
    {
        elapsed += Time.deltaTime;
        ApplyGravity();
        if (isAllowedMovement)
        {
            ApplyRotationAndMovement();
        }
        
        if (staminaManager.staminaBar.value == 0)
        {
            speed = walkingSpeed;
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropObjectHeld(false);
        }
        if (Input.GetKeyDown(KeyCode.Q) && isOnTeleportZone)
        {
            isTeleporting = true;
        }
        

        /*if (Input.GetKeyDown(KeyCode.Q) && isOnTeleportZone)
        {
            GoToLocation();
        }*/
        /*if (Input.GetKeyDown(KeyCode.C))
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


        }*/
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isNearbyVehicle && !isInVehicle)
            {
                Debug.Log("Entering Vehicle...");

                
                //enable car controls
                GameObject.Find("Car").GetComponent<PrometeoCarController>().enabled = true;

                //disable movement
                isAllowedMovement = false;

                //disable main cam
                ThirdPersonCamera.SetActive(false);


                isInVehicle = true;

                //set player invisible
                gameObject.GetComponent<MeshRenderer>().enabled = false;

                //set collider off
                gameObject.GetComponent<CapsuleCollider>().enabled = false;

                //transform.position = car.transform.position;
                return;
            }
            if (isInVehicle)
            {
                Debug.Log("Exit Vehicle");

                //disable car controls
                GameObject.Find("Car").GetComponent<PrometeoCarController>().enabled = false;

                //allow movement
                isAllowedMovement = true;

                //enable main cam
                ThirdPersonCamera.SetActive(true);

                isInVehicle = false;

                //set player visible
                gameObject.GetComponent<MeshRenderer>().enabled = true;

                //set collider on
                gameObject.GetComponent<CapsuleCollider>().enabled = true;

                //set player pos beside vehicle
                isExitingVehicle = true;
                
            }

        }
       /* if (isCrouching && isMoving)
        {
            Debug.Log("Crouch Walking...");
            animator.SetBool("isCrouchWalking", true);
        }
        else if (isCrouching && !isMoving)
        {
            Debug.Log("Not Crouch Walking");
            animator.SetBool("isCrouchWalking", false);
        }
*/

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
        if (isMoving && !isRunning)
        {
            if (elapsed > 0.35f)
            {
                PlayFootsteps();
                elapsed = 0;
            }
        }
        if (isMoving && isRunning)
        {
            if (elapsed > 0.25f)
            {
                PlayFootsteps();
                elapsed = 0;
            }
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
    private void PlayFootsteps()
    {
        playerAudioSource.PlayOneShot(feet[UnityEngine.Random.Range(0, feet.Count)]);

    }

    private void OnEnable()
    {


    }

    private void OnDisable()
    {

    }



    private void ApplyRotationAndMovement()
    {
        //movement from this tutorial: https://www.youtube.com/watch?v=4HpC--2iowE and the a good boy games channel
        if (input.sqrMagnitude == 0)
        {
            return;
        }


        //face direction of movement
        var targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y; //find angle require to move to get to point (x,y). cam.eulerangles.y takes into account cam rotation
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime); //smooth angle so it wont snap
        transform.rotation = Quaternion.Euler(0f, angle, 0f); //apply rotation

        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // turn rotation into direction by multiplying vector3.forward. by having cam.eulerangles.y, we move into the direction of cam rotation
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
        if (!isAllowedMovement)
        {
            return;
        }


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
        if (!isAllowedMovement)
            return;
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
            //animator.SetBool("isRunning", isRunning);

            speed += runSpeed; //isRunning is true if shift is held
            staminaManager.RegenStamina(false);

        }
        if (context.canceled && !context.performed)
        {
            Debug.Log("Player isn't Running...");
            isRunning = false;
            //animator.SetBool("isRunning", isRunning);
            speed -= runSpeed;
            staminaManager.RegenStamina(true);
        }

        //move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody or heavier than player or is interactable
        if (body == null || body.isKinematic || body.mass > gameObject.GetComponentInParent<Rigidbody>().mass || hit.gameObject.TryGetComponent(out IInteractable obj))
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }
        Vector3 forceDir = hit.gameObject.transform.position - transform.position;
        forceDir.y = 0;
        forceDir.Normalize();
        body.AddForceAtPosition(forceDir * (gameObject.GetComponentInParent<Rigidbody>().mass - body.mass) * speed, transform.position, ForceMode.Impulse);

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        //Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        // body.velocity = pushDir * pushPower;
    }
    private void OnTriggerStay(Collider other)
    {
        //for vehicle entry
        if (other.gameObject.name.Contains("Vehicle"))
        {
            
        }
        //for enter/exit triggers
        if (other.gameObject.name.Contains("Exit") || other.gameObject.name.Contains("Enter") || other.gameObject.name.Contains("Climb"))
        {
            if(teleportTrigger != other.gameObject.transform.GetChild(0).gameObject)
            {
                teleportTrigger = other.gameObject.transform.GetChild(0).gameObject;
            }
        }
        if (other.gameObject.TryGetComponent(out IInteractable interactable) && other.gameObject.name.Contains("Placeable") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("interacted!");
            interactable.Interact();
        }
        if (other.gameObject.name == "Map Guard")
        {
            BringPlayerBackToMap();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player Detection Range"))
        {
            isNearbyVehicle = true;
        }
        if (other.gameObject.name.Contains("Exit") || other.gameObject.name.Contains("Enter") || other.gameObject.name.Contains("Climb"))
        {
            //set label on
            other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            isOnTeleportZone = true;
        }
        
        

        if (other.gameObject.name.Contains("Placeable"))
        {
            isPlayerOnPlaceable = true;

        }
        
        if (other.tag == "Hazard")
        {
            DataHub.PlayerStatus.damageTaken = 10;
            TakeDamage(DataHub.PlayerStatus.damageTaken);
            //call healthchange event
            
            return;
        }
        
        

        //OBJECTIVE CONDITIONS
        if (other.gameObject.name == "Lobby")
        {
            DataHub.ObjectiveHelper.hasReachedLobby = true;
        }
        if(other.gameObject.name == "LevelWall")
        {
            DataHub.ObjectiveHelper.hasFoundLevel2Exit = true;
        }
        if (other.gameObject.name == "CarPark")
        {
            DataHub.ObjectiveHelper.hasFoundACar = true;
        }
        if (other.gameObject.name == "Towering Twins")
        {
            DataHub.ObjectiveHelper.hasReachedToweringTwins = true;
        }
        if (other.gameObject.name == "Evacuation Center")
        {
            DataHub.ObjectiveHelper.hasReachedEvacCenter = true;
        }
        if (other.gameObject.name == "Telephone Booth")
        {
            DataHub.ObjectiveHelper.hasFoundTelephone = true;
            
        }
        if (other.gameObject.name == "Higher Ground")
        {
            DataHub.ObjectiveHelper.hasReachedHigherGround = true;
            //trigger random event: earthquake then block exit
        }
        if (other.gameObject.name == "Chopper")
        {
            DataHub.ObjectiveHelper.hasReachedChopper = true;
        }


        /*if (other.isTrigger)
        {
            
            DataHub.TriggerInteracted.lastTriggerEntered = other.gameObject.name;
            EnterTrigger.Invoke();
        }*/

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player Detection Range"))
        {
            isNearbyVehicle = false;
        }
        if (other.gameObject.name.Contains("Exit") || other.gameObject.name.Contains("Enter") || other.gameObject.name.Contains("Climb"))
        {
            //set label off
            other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            isOnTeleportZone = false;
            teleportTrigger = null;
        }
        if (other.tag == "SpawnCP")
        {
            //Debug.Log("Player exited Spawn");
            //ExitTrigger.Invoke();
        }
        if (other.tag == "LobbyCP")
        {
            Debug.Log("Player exited Lobby");

        }
        if (other.gameObject.name.Contains("Placeable"))
        {
            isPlayerOnPlaceable = false;

        }

        //OBJECTIVE CONDITIONS
        if (other.gameObject.name == "Spawn")
        {
            DataHub.ObjectiveHelper.hasExitedSpawn = true;
        }
        /*if (other.isTrigger)
        {
            DataHub.TriggerInteracted.lastTriggerExited = other.gameObject.name;
            ExitTrigger.Invoke();
        }*/
        

    }
    public void TakeDamage(int damage)
    {
        TakeDamageEvent.Invoke();
        Debug.Log("Player is hurt!");
    }
    /*private Vector3 SetPlayerPosition(float x, float y, float z)
    {
        return;
    }*/
    public bool GetIsPlayerOnPlaceable()
    {
        return isPlayerOnPlaceable;
    }
    public void AddToListOfObjectsHeld(GameObject obj)
    {
        objectsHeld.Add(obj);
    }
    
    /*
     When player clicks e, for hold interactions, we grab the gameobject's name in the editor, then pass it to HoldObject().
     Ensure that we have a non empty object before adding to inventory. If it is an empty object, we are not holding anything, then return
        - EmptyObj occurs when focused inventory slot is empty

     Interactables that can be put in inventory must have an item data under scripts>inventory>items
     These item data must then be put in the hierarchy under Player>inventory interactables then drag and drop the item data into the array. This serves as the item data database, which lists props
        that can be placed into inventory.

     For placeable interactions, the naming convention must be [name of game object that matches placeable]Placeable.
        For example, we have a PlankPlaceable interaction in the first level and we need a Plank for it. The naming must be consistent. If we have ShortPlankPlaceable, we need ShortPlank for it.

 */
    public void HoldObject(string objName,GameObject obj)
    {
        if (objName == "EmptyObj")
        {
            return;
        }

        //add to inventory
        if (inv.Add(itemsArray.interactables.Find(item => item.displayName.StartsWith(objName))))
        {
            /*//make object disappear/inactive
            GameObject obj = itemsArray.itemGameObjects.Find(x => x.name == objName);
            obj.SetActive(false);*/
            AddToListOfObjectsHeld(obj);
            obj.SetActive(false);
        }
        objectHeld = GetObjectHeld();




    }
    /* When dropping an object, we get the item on focused inventory slot. If it is empty, we don't have any items to drop
     * otherwise, remove it from our inventory
     */
    public void DropObjectHeld(bool isPlaced)
    {
        //way to save inventory: instantiate object if it doesnt exist in scene but is in inventory
        if (GetObjectHeld() == "EmptyObj")
        {
            Debug.Log("No items to drop!");
            return;
        }
        


        if (!isPlaced)
        {
            
            GameObject obj = objectsHeld.Find(x => x.name == GetObjectHeld());

            //make object reappear beside player
            //GameObject obj = itemsArray.itemGameObjects.Find(x => x.name == objectHeld.Trim());
            obj.SetActive(true);

            obj.transform.position = new Vector3(gameObject.transform.position.x + 0.8f, gameObject.transform.position.y, gameObject.transform.position.z + 0.8f);


            obj.transform.rotation = gameObject.transform.rotation;

            objectsHeld.Remove(obj);

            
        }
        //remove from inventory
        
        inv.Remove(itemsArray.interactables.Find(item => item.displayName.Equals(objectHeld.Trim())));
        objectHeld = GetObjectHeld();

    }
    //retrieves item on focused inventory slot
    public string GetObjectHeld()
    {
        return inv.GetActiveItem();
    }
    
    //called from ChangeInventoryFocusEvent 
    public void SetObjectHeld()
    {
        objectHeld = inv.GetActiveItem();
        Debug.Log("Focused on " + objectHeld);
    }
    public GameObject GetGameObjectHeld()
    {
        return objectsHeld.Find(x => x.name == GetObjectHeld());
    }
    public void SetIsAllowedMovement(bool flag)
    {
        isAllowedMovement = flag;
    }
    private void BringPlayerBackToMap()
    {
        transform.position = new Vector3(transform.position.x, 4, transform.position.z);
    }
}
