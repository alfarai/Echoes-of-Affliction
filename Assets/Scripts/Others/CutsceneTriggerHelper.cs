using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTriggerHelper : MonoBehaviour
{
    public GameObject cutscene;
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            cutscene.SetActive(true);
        }
    }
}
