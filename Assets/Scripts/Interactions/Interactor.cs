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

    


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GetComponent<Character>();

    }
   

    // Update is called once per frame
    void LateUpdate()
    {
        //Ray ray = cam.ScreenPointToRay(pos);
        //Debug.DrawRay(ray.origin, ray.direction * InteractRange,Color.green);
        Debug.DrawRay(transform.position, cam.transform.forward * InteractRange, Color.green);
        if (Physics.Raycast(new Ray(transform.position, cam.transform.forward * InteractRange), out hitInfo))
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

