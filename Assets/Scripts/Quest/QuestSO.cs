using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class QuestSO : ScriptableObject
{
    public bool isQuestCompleted = false;
    public string questName;

    [Header("Finding Items Quest Settings")]
    public bool demandsItem = false;
    public Item itemToFind;

    [Header("Escort Quest Settings")]
    public bool escortQuest = false;
    public string destination;
}
