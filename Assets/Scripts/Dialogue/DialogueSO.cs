using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "ScriptableObjects/DialogueSO", order = 1)]
public class DialogueSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] dialogueLines;

    [Header("Animations")]
    public bool shouldUseFadeOut = false;
    public bool shouldUseFadeIn = false;

    [Header("Cutscene settings")]
    public bool isCutSceneDialogue = false;
    public bool shouldPlayCutscene = false;
    public bool shouldContinueCutscene = false;
    public bool shouldLoadScene = false;
    public string sceneToLoad = "";

    [Header("Settings")]
    public CharacterSO speakerData;
    public bool shouldGiveItem = false;
    public bool shouldFollowPlayerAfterDialogue = false;
    public bool shouldDisableNPCAfterDialogue = false;
    public bool shouldChangeDialogueAfterThis = false;

    [Header("Dialogue Changer")]
    public GameObject dialogueChangerToSpawn;

    [Header("Quest Settings")]
    public bool shouldGiveQuest = false;
    public bool shouldCompleteQuest = false;
    public QuestSO questToGive;
}
