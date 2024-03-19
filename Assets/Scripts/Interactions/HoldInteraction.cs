using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldInteraction : MonoBehaviour, IInteractable
{
    public GameObject playerObj;
    private Character player;
    public void Interact()
    {
        player.HoldObject(gameObject.name);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
