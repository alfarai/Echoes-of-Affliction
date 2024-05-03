using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCutscene : MonoBehaviour
{
    public GameObject cam;
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

        flash.FlashCutsceneMessage("The aftershocks caused a pile of rubble!");
        yield return new WaitForSeconds(5);
        flash.FlashCutsceneMessage("You must destroy the rubble to proceed.");
        yield return new WaitForSeconds(5);
        flash.FlashCutsceneMessage("Tip: Placing two objects close together may cause interactions between them...");
        yield return new WaitForSeconds(5);
        cam.SetActive(false);

        flash.HidePanel();
        cam.SetActive(false);
        player.SetIsAllowedMovement(true);
        DataHub.PlayerStatus.isCutscenePlaying = false;
        gameObject.SetActive(false);
    }



}
