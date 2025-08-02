using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;

    [Header("Dialogues Settings")]
    public DialogueSO[] dialogues;
    public int currentDialogueIndex = 0;
    public int currentInnerDialogueIndex = 0;

    [Header("Dialogue UI")]
    public GameObject dialogueUI;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject nextText;

    [Header("Typewriter Effect")]
    public float typewriterSpeed = 0.05f; // Time between each character

    private Coroutine typewriterCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nextText.activeSelf)
            {
                NextDialogue();
            }
        }
    }

    public void SetDialogues(DialogueSO[] newDialogues)
    {
        dialogues = newDialogues;
        currentDialogueIndex = 0;
        currentInnerDialogueIndex = 0;
    }

    public void ShowDialogue()
    {
        dialogueUI.SetActive(true);
        speakerNameText.text = dialogues[currentDialogueIndex].speakerData.characterName;

        // Stop any existing typewriter effect
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }

        // Start typewriter effect for the dialogue text
        typewriterCoroutine = StartCoroutine(TypewriterEffect(dialogues[currentDialogueIndex].dialogueLines[currentInnerDialogueIndex]));
    }

    private IEnumerator TypewriterEffect(string text)
    {
        dialogueText.text = "";

        foreach (char character in text)
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        // After finishing the typewriter effect, show the next text button
        nextText.SetActive(true);
    }

    public void NextDialogue()
    {
        // Check if there are more inner dialogue lines in the current dialogue
        if (currentInnerDialogueIndex < dialogues[currentDialogueIndex].dialogueLines.Length - 1)
        {
            // Move to next inner dialogue line
            currentInnerDialogueIndex++;
        }
        else
        {
            // Move to next dialogue and reset inner index
            currentDialogueIndex++;
            currentInnerDialogueIndex = 0;
        }

        nextText.SetActive(false);
        
        // Check if we've reached the end of all dialogues
        if (currentDialogueIndex >= dialogues.Length)
        {
           dialogueUI.SetActive(false);
            PlayerController.instance.isInteracting = false;
            return; // Important: exit the method here
        }
        
        // Show the next dialogue
        ShowDialogue();
    }

    public void CloseDialogue()
    {
        dialogueUI.SetActive(false);
        PlayerController.instance.isInteracting = false;
    }
}
