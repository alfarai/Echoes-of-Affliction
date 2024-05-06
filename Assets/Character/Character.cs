using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.SceneManagement;


public class Character : MonoBehaviour
{
    public Animator animator;
    private AudioManager audio;
    private StaminaManager staminaManager;
    private CharacterController characterController;
    private Inventory inv;
    private ItemsArray itemsArray;
    private Vector3 place;
    private bool isBlackScreenEnabled;

    private RaycastHit hit;

    private Vector2 input;
    private Vector3 move;
    private Vector3 moveDir;

    private GameObject teleportTrigger;

    public List<GameObject> objectsHeld = new List<GameObject>();


    private float gravity = -6f;

    private float verticalVelocity, previousVerticalVelocity;
    private float currentVelocity;
    private bool hasJumped, hasLanded, isFalling, isRunning, isWalking, isAllowedMovement = true;
    private bool isPlayerOnPlaceable = false;
    private float speedTemp;

    //public Vector3 objectPosition;
    public string objectHeld = "EmptyObj";
    public bool isOnTeleportZone, isTeleporting, isNearbyVehicle, isInVehicle, isExitingVehicle;
    public static event Action onJump;

    public Transform camera;
    public GameObject ThirdPersonCamera;
    public GameObject FocusCamera;
    public GameObject itemsArrayObj;
    public float pushPower = 2.0f;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float playerVelocity = 5f, walkingSpeed = 5f, sprintingSpeed = 9f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private float fallThresholdVelocity = 5f;

    public UnityEvent ExitTrigger, EnterTrigger, TakeDamageEvent;
    private GameObject car;
    private float elapsed, timeSinceLastJump;
    public List<AudioClip> feet;
    public AudioClip hurt, dead;
    //public AudioClip outOfBreath;
    private AudioSource playerAudioSource;
    private GameObject RobModel;
    private bool canJump = true, isCameraFocused, canPlayOutOfBreath;
    public float jumpInterval = 1f;
    public CanvasGroup blackScreenCanvas;
    private FlashInfo flash;

    private void Awake()
    {

    }


    void Start()
    {
        flash = GameObject.Find("FlashMessager").GetComponent<FlashInfo>();
        //animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        //hpScript = hpObj.GetComponent<UpdateHP>();
        //hp = hpScript.GetHP(); //set hp
        speedTemp = playerVelocity;
        //animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        inv = GetComponent<Inventory>();
        itemsArray = itemsArrayObj.GetComponent<ItemsArray>();
        staminaManager = GetComponent<StaminaManager>();
        //objectPosition = transform.position;
        car = GameObject.Find("Car");
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        playerAudioSource = gameObject.GetComponent<AudioSource>();
        RobModel = GameObject.Find("Rob_Animated");
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
            GameObject car = GameObject.Find("Car Entrance");
            transform.position = new Vector3(car.transform.position.x, car.transform.position.y, car.transform.position.z);
            transform.rotation = car.transform.rotation;
            if (transform.position.x >= car.transform.position.x)
            {
                isExitingVehicle = false;
            }


        }

    }
    // Update is called once per frame
    private void Update()
    {

        timeSinceLastJump += Time.deltaTime;
        if(timeSinceLastJump >= jumpInterval)
        {
            canJump = true;
        }
        

        if ((isWalking && hasJumped) || (isRunning && hasJumped))
        {
            animator.SetBool("isRunningJump", true);
            StartCoroutine(ResetRunningJumpFlagAfterDelay());
        }
        if (animator.GetBool("isJumping"))
        {
            StartCoroutine(ResetJumpFlagAfterDelay());
        }


        //applying fall damage on isGrounded
        if (characterController.isGrounded)
        {
            if (previousVerticalVelocity < -fallThresholdVelocity)
            {
                DataHub.PlayerStatus.damageTaken = Mathf.Abs(previousVerticalVelocity + fallThresholdVelocity) * 10; //damage is too small so just multiply by 10
                TakeDamage(DataHub.PlayerStatus.damageTaken);
                previousVerticalVelocity = verticalVelocity;
            }
        }
        elapsed += Time.deltaTime;
        ApplyGravity();
        if (isAllowedMovement || !isInVehicle)
        {
            ApplyRotationAndMovement();
        }


        if (!staminaManager.CanSprint())
        {
            SetPlayerSpeed(walkingSpeed);
            isRunning = false;
            animator.SetBool("isRunning", isRunning);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropObjectHeld(false);
        }
        if (Input.GetKeyDown(KeyCode.Q) && isOnTeleportZone)
        {
            isTeleporting = true;
        }
        //for camera fov change when running

        //zoom in
        if (Input.GetMouseButtonDown(1) && !isInVehicle)
        {
            ThirdPersonCamera.SetActive(false);
            FocusCamera.SetActive(true);
            isCameraFocused = true;
        }
        //zoom out
        if (Input.GetMouseButtonUp(1) && !isInVehicle)
        {
            ThirdPersonCamera.SetActive(true);
            FocusCamera.SetActive(false);
            isCameraFocused = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isNearbyVehicle && !isInVehicle)
            {
                Debug.Log("Entering Vehicle...");

                //enable car controls
                car.GetComponent<PrometeoCarController>().enabled = true;
                car.GetComponent<AudioSource>().enabled = true;

                //disable movement
                isAllowedMovement = false;

                //disable main cam
                ThirdPersonCamera.SetActive(false);

                isInVehicle = true;
                RobModel.SetActive(false);
                return;
            }
            if (isInVehicle)
            {
                Debug.Log("Exit Vehicle");

                //disable car controls
                car.GetComponent<PrometeoCarController>().enabled = false;
                car.GetComponent<AudioSource>().enabled = false;

                //allow movement
                isAllowedMovement = true;
                //enable main cam
                ThirdPersonCamera.SetActive(true);

                isInVehicle = false;
                RobModel.SetActive(true);

                //set player pos beside vehicle
                isExitingVehicle = true;

            }

        }


        //sound to play if player is not running
        if (!DataHub.PlayerStatus.isCutscenePlaying)
        {
            if (isWalking && !isRunning && !isInVehicle)
            {
                if (elapsed > 0.35f)
                {
                    PlayFootsteps();
                    elapsed = 0;
                }
            }
            //sound to play if player is running
            if (isWalking && isRunning && !isInVehicle)
            {
                if (elapsed > 0.25f)
                {
                    PlayFootsteps();
                    elapsed = 0;
                }
            }
        }

        /*//sound to play if player is about to run out of stamina
        //this ensures that it only plays once when player stamina is <50
        if (staminaManager.isStaminaHalfway() && canPlayOutOfBreath)
        {
            PlayOutOfBreath();
            canPlayOutOfBreath = false;
        }
        if (staminaManager.isStaminaMoreThanHalf())
        {
            canPlayOutOfBreath = true;
        }*/


        if (isBlackScreenEnabled)
        {
            blackScreenCanvas.alpha = Mathf.MoveTowards(blackScreenCanvas.alpha, 1, 0.8f * Time.deltaTime);
        }





    }
    private void PlayFootsteps()
    {
        playerAudioSource.PlayOneShot(feet[UnityEngine.Random.Range(0, feet.Count)]);

    }
    private void PlayOutOfBreath()
    {
        //playerAudioSource.PlayOneShot(outOfBreath);
    }
    public void PlayHurt()
    {
        playerAudioSource.PlayOneShot(hurt);
    }
    public void PlayDead()
    {
        playerAudioSource.PlayOneShot(dead);
    }


    private void ApplyRotationAndMovement()
    {
        //movement from this tutorial: https://www.youtube.com/watch?v=4HpC--2iowE and the a good boy games channel
        if (input.sqrMagnitude == 0)
        {
            return;
        }


        //face direction of movement
        var targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + camera.eulerAngles.y; //find angle require to move to get to point (x,y). cam.eulerangles.y takes into account cam rotation
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime); //smooth angle so it wont snap
        transform.rotation = Quaternion.Euler(0f, angle, 0f); //apply rotation

        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // turn rotation into direction by multiplying vector3.forward. by having cam.eulerangles.y, we move into the direction of cam rotation
        moveDir.y = verticalVelocity;
        //movement
        characterController.Move(moveDir * Time.deltaTime * playerVelocity);


    }
    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -1.0f;

        }
        else
        {

            verticalVelocity += gravity * Time.deltaTime;
            previousVerticalVelocity = verticalVelocity;
        }
        moveDir = Vector3.zero;
        moveDir.y = verticalVelocity;
        characterController.Move(moveDir * Time.deltaTime * playerVelocity);
        //reset moveDir so that it only applies gravity

    }


    //this method is called in playerinput object events
    public void Move(InputAction.CallbackContext context)
    {

        if (context.canceled && !context.performed)
        {

            isWalking = false;
            isRunning = false;
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isRunning", isRunning);
            if (!isWalking)
            {
                //    Debug.Log("Not Crouch Walking");
                //animator.SetBool("isCrouchWalking", false);
                playerVelocity = speedTemp;
            }

        }
        if (context.started && !context.performed)
        {
            isWalking = true;
            animator.SetBool("isWalking", isWalking);

        }
        if (isAllowedMovement)
            input = context.ReadValue<Vector2>();
        else
        {
            input = Vector2.zero;
            isWalking = false;
            isRunning = false;
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isRunning", isRunning);
        }
            
        //  Debug.Log(input);
        move = new Vector3(input.x, 0, input.y).normalized;

        //move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    }
    public void Jump(InputAction.CallbackContext context)
    {
        //https://forum.unity.com/threads/learning-new-inputsystem-when-and-how-is-the-cancelled-callback-used.969501/
        if (!isAllowedMovement)
        {
            return;
        }
        if (context.performed && !hasJumped && canJump && characterController.isGrounded && (!IsJumpAnimationPlaying() || !IsRunningJumpAnimationPlaying()))
        {
            hasJumped = true;
            animator.SetBool("isJumping", hasJumped);
            verticalVelocity += jumpPower;
            canJump = false;
            timeSinceLastJump = 0f;
        }
    }
    public void Run(InputAction.CallbackContext context)
    {
        if (!isAllowedMovement || !isWalking)
        {
            return;
        }
        if (isWalking)
        {
            if (context.started)
            {

                isRunning = true;
                animator.SetBool("isRunning", isRunning);
                SetPlayerSpeed(sprintingSpeed);
                staminaManager.ShiftHeld(true);
                StartCoroutine(ChangeFOV(ThirdPersonCamera.GetComponent<CinemachineFreeLook>(), 50, 0.5f));
            }
            if (context.canceled && !context.performed)
            {

                isRunning = false;
                animator.SetBool("isRunning", isRunning);
                SetPlayerSpeed(walkingSpeed);
                staminaManager.ShiftHeld(false);
                StartCoroutine(ChangeFOV(ThirdPersonCamera.GetComponent<CinemachineFreeLook>(), 40, 0.5f));
            }
        }


        //move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    }
    bool IsJumpAnimationPlaying()
    {
        // Get the current state of the jump animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // Check if the jump animation is playing
        return stateInfo.IsName("Jumping"); // Replace "YourJumpAnimationName" with the actual name of your jump animation state
    }
    bool IsRunningJumpAnimationPlaying()
    {
        // Get the current state of the jump animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // Check if the jump animation is playing
        return stateInfo.IsName("Running Jump"); // Replace "YourJumpAnimationName" with the actual name of your jump animation state
    }
    IEnumerator EnableBlackScreen()
    {
        SetIsAllowedMovement(false);
        DataHub.PlayerStatus.isCutscenePlaying = true;
        isBlackScreenEnabled = true;
        yield return new WaitForSeconds(6); //let black screen show for a few seconds
        //show main menu
        SceneManager.LoadSceneAsync("MainMenu");
        /*isBlackScreenEnabled = false;
        blackScreenCanvas.alpha = 0f;*/
    }
    //Example coroutine to reset isJumping after a delay(if animation event approach is not feasible)
    IEnumerator ResetJumpFlagAfterDelay()
    {
        yield return new WaitForSeconds(1.517f);
        hasJumped = false;
        animator.SetBool("isJumping", hasJumped);
    }
    IEnumerator ResetRunningJumpFlagAfterDelay()
    {
        yield return new WaitForSeconds(0.867f);
        hasJumped = false;
        animator.SetBool("isRunningJump", false);
    }
    IEnumerator ChangeFOV(CinemachineFreeLook cam, float endFOV, float duration)
    {

        float startFOV = cam.m_Lens.FieldOfView;
        float time = 0;
        while (time < duration)
        {
            cam.m_Lens.FieldOfView = Mathf.Clamp(Mathf.Lerp(startFOV, endFOV, time / duration), 40, 50);
            yield return null;
            time += Time.deltaTime;
        }
    }
    private void SetPlayerSpeed(float var)
    {
        playerVelocity = var;
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
        body.AddForceAtPosition(forceDir * (gameObject.GetComponentInParent<Rigidbody>().mass - body.mass) * playerVelocity, transform.position, ForceMode.Impulse);

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
            if (teleportTrigger != other.gameObject.transform.GetChild(0).gameObject)
            {
                teleportTrigger = other.gameObject.transform.GetChild(0).gameObject;
            }
        }
        if (other.gameObject.name == "Map Guard")
        {
            BringPlayerBackToMap();
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name.Contains("Car Entrance"))
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
        if(other.name == "NPC Talk Instruction")
        {
            StartCoroutine(flash.FlashMessage("You can interact with strangers by pressing E on them.", 5));
            other.gameObject.SetActive(false);
        }
        if(other.name == "PlayFirstBG")
        {
            audio.PlayMusic(audio.bg1);
        }
        if(other.name == "Chainsaw Tree Helper")
        {
            StartCoroutine(flash.FlashMessage("You can break down the fallen trees with a chainsaw.", 5));
            other.gameObject.SetActive(false);
        }



        //OBJECTIVE CONDITIONS
        if (other.gameObject.name == "Lobby")
        {
            DataHub.ObjectiveHelper.hasReachedLobby = true;
        }
        if (other.gameObject.name == "LevelWall")
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
            StartCoroutine(EnableBlackScreen());
        }


        /*if (other.isTrigger)
        {
            
            DataHub.TriggerInteracted.lastTriggerEntered = other.gameObject.name;
            EnterTrigger.Invoke();
        }*/

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Car Entrance"))
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
    public void TakeDamage(float damage)
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
    public void HoldObject(string objName, GameObject obj)
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
        audio.PlaySFX(audio.itemHold);



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
            obj.GetComponent<Rigidbody>().useGravity = true;
            obj.transform.position = new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z + 0.4f);
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

    //Disable/Enable of Cameras used for when game screens are shown (ex: death, pause)
    public void DisableCameraRotation()
    {
        ThirdPersonCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0;
        ThirdPersonCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0;
    }
    public void EnableCameraRotation()
    {
        ThirdPersonCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300; //default
        ThirdPersonCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 3; //default
    }
    public bool GetIsCameraFocused()
    {
        return isCameraFocused;
    }
}
