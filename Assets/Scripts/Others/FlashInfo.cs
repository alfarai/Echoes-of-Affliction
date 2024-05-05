using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashInfo: MonoBehaviour
{
    public GameObject infoObj;
    public TextMeshProUGUI info;
    
    void Start()
    {
       
    }
    public IEnumerator FlashMessage(string msg,int time)
    {

        ShowPanel();
        info.text = msg;
        yield return new WaitForSeconds(time);
        HidePanel();
    }
    private void HideMessage()
    {
        infoObj.SetActive(false);
    }
    //show of panel is handled by the cutscene script, as well as the wait time before each flash messages
    public void FlashCutsceneMessage(string msg)
    {
        info.text = msg;
    } 
    public void ShowPanel()
    {
        if (!infoObj.activeInHierarchy)
        {
            infoObj.SetActive(true);
        }
        
    }
    public void HidePanel()
    {
        if (infoObj.activeInHierarchy)
        {
            infoObj.SetActive(false);
        }
        
    }

}
