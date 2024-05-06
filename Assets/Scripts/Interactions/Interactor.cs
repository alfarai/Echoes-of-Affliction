using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    private Vector3 pos = new Vector3(0, 0, 0);

    private Camera cam;
    private Character player;
    private RaycastHit hitInfo;
    private Vector3 rayPos;
    


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GetComponent<Character>();
        rayPos = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
    }
   

    // Update is called once per frame
    void LateUpdate()
    {
        //Ray ray = cam.ScreenPointToRay(pos);
        //Debug.DrawRay(ray.origin, ray.direction * InteractRange,Color.green);
        rayPos = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        Debug.DrawRay(rayPos, cam.transform.forward * InteractRange, Color.green);
        if (Physics.Raycast(new Ray(rayPos, cam.transform.forward * InteractRange), out hitInfo))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                if (hitInfo.collider.gameObject.name.Contains("Placeable"))
                    return;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    
                    //Debug.Log("Interacting with " + hitInfo.collider.gameObject.name);
                    interactObj.Interact();
                    
                }

            }
        }
    }
    
    
}

