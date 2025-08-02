using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDialogueChanger : MonoBehaviour
{
    public DialogueSO[] dialogues;
    void OnEnable()
    {
        DialogueController.instance.currentDialogueIndex = 0;
        DialogueController.instance.currentInnerDialogueIndex = 0;

        DialogueController.instance.cutsceneDirector.Pause();
        DialogueController.instance.dialogues = dialogues;
        
        if (dialogues[0].shouldUseFadeIn)
        {
            DialogueController.instance.FadeIn();
        }
        DialogueController.instance.ShowDialogue();
    }
}
