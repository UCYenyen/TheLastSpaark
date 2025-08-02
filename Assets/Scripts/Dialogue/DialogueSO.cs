using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "ScriptableObjects/DialogueSO", order = 1)]
public class DialogueSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] dialogueLines;

    [Header("Settings")]
    public CharacterSO speakerData;
}
