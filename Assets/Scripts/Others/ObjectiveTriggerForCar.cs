using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTriggerForCar : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        //OBJECTIVE CONDITIONS
        if (other.gameObject.name == "Lobby")
        {
            DataHub.ObjectiveHelper.hasReachedLobby = true;
        }
        if (other.gameObject.name == "LevelWall")
        {
            DataHub.ObjectiveHelper.hasFoundLevel2Exit = true;
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
        }
    }
}
