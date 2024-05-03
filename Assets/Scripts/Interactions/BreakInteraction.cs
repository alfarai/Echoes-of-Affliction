using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class BreakInteraction : MonoBehaviour, IInteractable
{
    public int hitsBeforeBreaking = 0;
    public UnityEvent BreakEvent;
    public float timeIntervalBeforeNextHit = 1f;
    private float elapsedTimeSinceLastHit = -1f; //-1f = no hits, 0f = object was hit
    private Character player;
    public GameObject tooltipPrefab;
    private GameObject clone;
    private Vector3 labelPos;
    private bool canHit = true, isOutlineActive;
    private FlashInfo flash;
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
            if (!canHit)
            {
                StartCoroutine(flash.FlashMessage("Break is on cooldown!",2));
               
            }
            if (canHit)
            {
                --hitsBeforeBreaking;
                elapsedTimeSinceLastHit = 0;



                //break object if 0
                if (hitsBeforeBreaking == 0)
                {
                    DestroyLabel();
                    gameObject.SetActive(false);
                    Debug.Log("Object broken!");
                    DataHub.ObjectInteracted.objectBroken = gameObject.name;
                    BreakEvent.Invoke();
                }
                else if (hitsBeforeBreaking > 0)
                {
                    Debug.Log("Object hit! " + "(" + hitsBeforeBreaking + " left)");
                }
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
        flash = GameObject.Find("FlashMessager").GetComponent<FlashInfo>();
        labelPos = transform.position;
        labelPos.y -= 1.1f;
        player = GameObject.Find("Player").GetComponent<Character>();
        
    }
    void Update()
    {
        if(elapsedTimeSinceLastHit >= 0)
        {
            canHit = false;
            elapsedTimeSinceLastHit += Time.deltaTime;
        }
        if(elapsedTimeSinceLastHit >= timeIntervalBeforeNextHit)
        {
            canHit = true;
            elapsedTimeSinceLastHit = -1f;

        }
    }


    public void InstantiateLabel()
    {
        clone = Instantiate(tooltipPrefab, labelPos, Quaternion.identity);
        clone.GetComponentInChildren<TextMesh>().text = "Break";

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
