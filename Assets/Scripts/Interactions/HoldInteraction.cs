using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoldInteraction : MonoBehaviour, IInteractable
{
    //public GameObject playerObj;
    public UnityEvent HoldEvent;
    private Character player;
    public GameObject tooltipPrefab;
    private GameObject clone;
    private Vector3 labelPos;
    private bool isOutlineActive;
    public void Interact()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        player.HoldObject(gameObject.name,gameObject);
        DestroyLabel();


        /*DataHub.ObjectInteracted.interactedObj = gameObject;
        DataHub.ObjectInteracted.interactable = player.GetObjectHeld();
        DataHub.ObjectInteracted.interaction = "hold";*/

        DataHub.ObjectInteracted.objectHeld = player.GetObjectHeld();
        HoldEvent.Invoke();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        labelPos = transform.position;
        labelPos.y -= 1.1f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    void FixedUpdate()
    {
        if (labelPos != transform.position)
        {
            labelPos = transform.position;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    
    public void InstantiateLabel()
    {
        clone = Instantiate(tooltipPrefab,labelPos,Quaternion.identity);
        //clone.GetComponentInChildren<TextMesh>().text = gameObject.name;
        
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
