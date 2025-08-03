using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestController : MonoBehaviour
{
    public static QuestController instance;

    [Header("Quest Data")]
    public List<QuestSO> activeQuests = new List<QuestSO>();

    [Header("UI Elements")]
    public Transform questTextUIContainer;
    public TMP_Text textPrefab;
    public List<TMP_Text> questTextUIList = new List<TMP_Text>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddQuest(QuestSO newQuest)
    {
        if (!activeQuests.Contains(newQuest))
        {
            activeQuests.Add(newQuest);
            TMP_Text questTextUI = Instantiate(textPrefab, questTextUIContainer);
            questTextUI.text = newQuest.questName;
            questTextUIList.Add(questTextUI);
        }
    }
    public void CompleteQuest(QuestSO quest)
    {
        if (activeQuests.Contains(quest))
        {
            quest.isQuestCompleted = true;
            activeQuests.Remove(quest);
            TMP_Text questTextUI = questTextUIList.Find(q => q.text == quest.questName);
            if (questTextUI != null)
            {
                questTextUIList.Remove(questTextUI);
                Destroy(questTextUI.gameObject);
            }
            // Additional logic for completing the quest can be added here, like giving rewards.
        }
    }
}
