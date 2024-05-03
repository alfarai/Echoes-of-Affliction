using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    public float detectionRadius = 1;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(detectionRadius, detectionRadius, detectionRadius);
    }

    
    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.InstantiateLabel();
            interactable.EnableOutline(true);

        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.DestroyLabel();
            interactable.EnableOutline(false);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable) && other.gameObject.name.Contains("Placeable") && Input.GetKeyDown(KeyCode.E))
        {
            interactable.Interact();
        }
    }
}
