using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerFailedToBringBandageCutscene : MonoBehaviour
{
    public GameObject cam, cam1, cam2;
    public GameObject dialogueTextObj;
    public TextMeshProUGUI dialogueText;
    public CanvasGroup blackScreenCanvas;
    public CollapseBuilding collapse;
    private FlashInfo flash;
    private IEnumerator coroutine;
    private Character player;
    private bool isBlackScreenEnabled;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Character>();
        StartCoroutine(Cutscene());
    }
    IEnumerator Cutscene()
    {
        DataHub.PlayerStatus.isCutscenePlaying = true;
        player.SetIsAllowedMovement(false);
        cam.SetActive(true);
        dialogueTextObj.SetActive(true);
       

        dialogueText.text = "*Grunts in pain* I think I can't hold on anymore...";
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = "*Building starts to weaken*";
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = "This is it for me...";
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = "I will never forget you, Robert.";
        yield return new WaitForSeconds(3.5f);
        dialogueTextObj.SetActive(false);
        cam.SetActive(false);

        cam1.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        collapse.enabled = true;

        yield return new WaitForSeconds(2);
        cam1.SetActive(false);

        cam2.SetActive(true);
        yield return new WaitForSeconds(7f); //collapse time is 6 seconds

        //isBlackScreenEnabled = true;

        isBlackScreenEnabled = true;
        yield return new WaitForSeconds(2); //let black screen show for a few seconds
        isBlackScreenEnabled = false;
        blackScreenCanvas.alpha = 0f;


        cam2.SetActive(false);

        yield return new WaitForSeconds(2);
        cam.SetActive(false);
        player.SetIsAllowedMovement(true);
        DataHub.PlayerStatus.isCutscenePlaying = false;
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (isBlackScreenEnabled)
        {
            blackScreenCanvas.alpha = Mathf.MoveTowards(blackScreenCanvas.alpha, 1, 1.0f * Time.deltaTime);
        }
    }



}
