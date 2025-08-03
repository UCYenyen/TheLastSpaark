using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChanger : MonoBehaviour
{
    public bool shouldTalkImmediately = false;
    public DialogueSO[] dialogues;
    public string npcName;
    public bool shouldChangeTheDialogueComponentInAnNPC = false;
    void OnEnable()
    {
        DialogueController.instance.currentDialogueIndex = 0;
        DialogueController.instance.currentInnerDialogueIndex = 0;

        DialogueController.instance.dialogues = dialogues;

        if (shouldTalkImmediately)
        {
            DialogueController.instance.ShowDialogue();
        }
        
        if(shouldChangeTheDialogueComponentInAnNPC)
        {
            GameManager.instance.FindNPC(npcName).dialogueData = dialogues;
        }
    }
}
