using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IInteractable
{
    public HealthManager playerHealth;
    string playerObjectName = "Capsule";
    private void Start()
    {
        playerHealth = GameObject.Find(playerObjectName).GetComponent<HealthManager>();
    }
    public void Interact()
    {

        playerHealth.TakeDamage(10);

        Debug.Log("hello");
    }
}
