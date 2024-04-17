using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class DialogueController : MonoBehaviour
{
    private Queue<string> paragraphs = new Queue<string>();
    private bool isConversationOver,isInitialized,hasEndedConversation;
    
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    private string p;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowNextParagraph(DialogueText dialogue)
    {
        //if paragraphs to display is not initialized
        if(paragraphs.Count == 0)
        {
            if (!isConversationOver)
            {
                
                InitConversation(dialogue);
            }
            else
            {
                EndConversation();
                return;
            }
        }
        //if paragraphs are initialize, display them
        p = paragraphs.Dequeue();
        dialogueText.text = p;

        //if no more paragraphs to display, end conversation
        if(paragraphs.Count == 0)
        {
            isConversationOver = true;
            
        }

    }
   
    private void InitConversation(DialogueText dialogue)
    {
        hasEndedConversation = false;
        gameObject.SetActive(true);
        for (int i = 0; i < dialogue.paragraphs.Length; i++)
        {
            paragraphs.Enqueue(dialogue.paragraphs[i]);
        }
        isInitialized = true;
    }
    private void EndConversation()
    {
        hasEndedConversation = true;
        isInitialized = false;
        isConversationOver = false;
        gameObject.SetActive(false);
    }
    public bool GetIsInitialized()
    {
        return isInitialized;
    }
    public bool GetIsConversationOver()
    {
        return isConversationOver;
    }
    public bool GetHasEndedConversation()
    {
        return hasEndedConversation;
    }
    
}
