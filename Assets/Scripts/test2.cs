using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log(DataHub.PlayerStatus.health);
        Debug.Log(DataHub.PlayerStatus.playerInventory[0].GetItemDataName());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
