using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorShowEntry : MonoBehaviour
{
    public GameObject closedDoor, openedDoor, keys, newPlaceableKeys,oldPlaceableKeys;
    private bool flag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DataHub.ObjectiveHelper.hasKeyBeenPlaced && DataHub.ObjectiveHelper.hasBarrierBeenBroken && !flag)
        {
            flag = true;
            closedDoor.SetActive(false);
            openedDoor.SetActive(true);
            oldPlaceableKeys.SetActive(false);
            keys.transform.position = newPlaceableKeys.transform.position;
            keys.transform.rotation = newPlaceableKeys.transform.rotation;
            keys.GetComponent<Rigidbody>().useGravity = true;
            keys.GetComponent<Collider>().enabled = false;
            keys.GetComponent<PlaceInteraction>().enabled = false;
        }
    }
}
