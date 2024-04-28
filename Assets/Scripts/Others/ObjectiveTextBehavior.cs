using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjectiveTextBehavior : MonoBehaviour
{
    public GameObject UIElementsToHideOnEmptyText;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(text.text == ""){
            UIElementsToHideOnEmptyText.SetActive(false);
        }
        else
        {
            UIElementsToHideOnEmptyText.SetActive(true);
        }
    }
}
