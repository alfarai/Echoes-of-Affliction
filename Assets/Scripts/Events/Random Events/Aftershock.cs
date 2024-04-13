﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Aftershock : IEvent
{

    public GameObject rocksToSpawn;
    public GameObject buildingsToDestroy;
    public GameObject fallenBuilding,fallenBuilding2;
    protected override void PerformAction()
    {
        Debug.Log("Aftershock triggered!");
        //camshake https://www.youtube.com/watch?v=ACf1I27I6Tk&ab_channel=CodeMonkey
        CinemachineFreeLook cam = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
        
        //block exit (spawn rocks)
        rocksToSpawn.SetActive(true);
        buildingsToDestroy.SetActive(false);

        //tilt buildings
        fallenBuilding.transform.rotation = Quaternion.Euler(-70, 90, 0);
        fallenBuilding2.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    protected override bool ShouldTrigger(Collider other)
    {
        return other.CompareTag("Player");
    }


}