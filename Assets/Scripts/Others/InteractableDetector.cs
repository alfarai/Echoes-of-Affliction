using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    public float detectionRadius = 1;
    public float yOffset = 2;
    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }

    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.InstantiateLabel();
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.DestroyLabel();
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.TryGetComponent(out IInteractable interactable))
        {
            
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
