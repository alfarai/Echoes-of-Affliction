using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Aftershock : IEvent
{
    private AudioManager audio;
    public GameObject rocksToSpawn;
    public GameObject ObjectsToDelete;
    //public GameObject buildingsToDestroy;
    public GameObject[] buildingsToDestroy;
    private FlashInfo flash;
    protected override void PerformAction()
    {
        Debug.Log("Aftershock triggered!");
        flash = GameObject.Find("FlashMessager").GetComponent<FlashInfo>();
        StartCoroutine(flash.FlashMessage("There's an earthquake!", 5));
        DataHub.WorldEvents.hasFirstAftershockHappened = true;
        //camshake https://www.youtube.com/watch?v=ACf1I27I6Tk&ab_channel=CodeMonkey
        //CinemachineFreeLook cam = GameObject.Find("Move Camera").GetComponent<CinemachineFreeLook>();

        ObjectsToDelete.SetActive(false);
        //block exit (spawn rocks)
        rocksToSpawn.SetActive(true);
        //buildingsToDestroy.SetActive(false);

        //tilt buildings
        foreach(GameObject bldg in buildingsToDestroy)
        {
            if (bldg.TryGetComponent<CollapseBuilding>(out CollapseBuilding collapse))
            {
                collapse.enabled = true;
            }
        }
        audio.PlaySFX(audio.rumble);
        //fallenBuilding.transform.rotation = Quaternion.Euler(-70, 90, 0);
        //fallenBuilding2.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    protected override bool ShouldTrigger(Collider other)
    {
        Debug.Log(other.name);
        return other.CompareTag("Player") || other.CompareTag("Car");
    }
    void Start()
    {
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }


}
