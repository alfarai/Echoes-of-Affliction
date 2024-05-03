using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFoundCutscene : MonoBehaviour
{
    public GameObject cam, cam1;
    private FlashInfo flash;
    private IEnumerator coroutine;
    private Character player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Character>();
        StartCoroutine(Cutscene());
    }
    IEnumerator Cutscene()
    {
        DataHub.PlayerStatus.isCutscenePlaying = true;
        flash = GameObject.Find("FlashMessager").GetComponent<FlashInfo>();
        player.SetIsAllowedMovement(false);
        cam.SetActive(true);
        
        flash.ShowPanel();

        flash.FlashCutsceneMessage("A car is now available!");
        yield return new WaitForSeconds(3);
        flash.FlashCutsceneMessage("To enter and exit the car, Press F.");
        yield return new WaitForSeconds(3);
        flash.FlashCutsceneMessage("Use WASD to drive the car.");
        yield return new WaitForSeconds(3);
        cam.SetActive(false);
        cam1.SetActive(true);
        flash.FlashCutsceneMessage("The car is out of fuel! You need to gas it up first.");
        yield return new WaitForSeconds(5);
        cam1.SetActive(false);

        flash.HidePanel();
        cam.SetActive(false);
        player.SetIsAllowedMovement(true);
        DataHub.PlayerStatus.isCutscenePlaying = false;
        gameObject.SetActive(false);
    }



}
