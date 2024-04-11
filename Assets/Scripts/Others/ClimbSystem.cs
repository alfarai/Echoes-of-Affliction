using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbSystem : MonoBehaviour
{
    public Transform climbDestination;
    Character c;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>())
        {
            c = other.GetComponent<Character>();
            Debug.Log("Player entered the trigger zone.");
            Debug.Log("Current player position: " + other.transform.position);
            Debug.Log("Climb destination position: " + climbDestination.position);

           c.isOnTeleportZone = true;
           //c.objectPosition = new Vector3(climbDestination.position.x, climbDestination.position.y, climbDestination.position.z);

            Debug.Log("Player teleported to climb destination.");
            Debug.Log("New player position: " + other.transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (c != null)
        {
            c.isOnTeleportZone = false;
            
        }
    }


}
