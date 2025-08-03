using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using Unity.VisualScripting;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;

    [Header("Dialogues Settings")]
    public DialogueSO[] dialogues;
    public int currentDialogueIndex = 0;
    public int currentInnerDialogueIndex = 0;

    [Header("Dialogue UI")]
    public CanvasGroup dialogueUI;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject nextText;

    [Header("Typewriter Effect")]
    public float typewriterSpeed = 0.05f; // Time between each character
    private Coroutine typewriterCoroutine;

    [Header("FadeInFadeOut")]
    bool fadeIn = false;
    bool fadeOut = false;

    [Header("Cutscene")]
    public PlayableDirector cutsceneDirector;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "IntroCutscene")
        {
            ShowDialogue();
        }
    }
    void Update()
    {
        CalculateFadeInFadeOut();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nextText.activeSelf)
            {
                NextDialogue();
            }
        }
    }
    public void FadeIn()
    {
        fadeIn = true;
    }
    public void FadeOut()
    {
        fadeOut = true;
    }
    public void CalculateFadeInFadeOut()
    {
        if (fadeIn)
        {
            if (dialogueUI.alpha < 1f)
            {
                dialogueUI.alpha += Time.deltaTime;
                if (dialogueUI.alpha >= 1f)
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (dialogueUI.alpha > 0f)
            {
                dialogueUI.alpha -= Time.deltaTime;
                if (currentDialogueIndex > 0)
                {
                    if (dialogues[currentDialogueIndex - 1].shouldPlayCutscene)
                    {
                        if (dialogueUI.alpha <= 0.8f)
                        {
                            if (dialogues[currentDialogueIndex - 1].shouldContinueCutscene)
                            {
                                cutsceneDirector.Resume();
                            }
                            else
                            {
                                cutsceneDirector.Play();
                            }
                        }
                    }

                    if (dialogues[currentDialogueIndex - 1].shouldLoadScene)
                    {
                        if (dialogues[currentDialogueIndex - 1].dialogueLines.Length > 0)
                        {
                            if (currentInnerDialogueIndex < dialogues[currentDialogueIndex].dialogueLines.Length - 1)
                            {

                            }
                            else
                            {
                                if (dialogueUI.alpha <= 0.8f)
                                {
                                    SceneManager.LoadScene(dialogues[currentDialogueIndex - 1].sceneToLoad);
                                }
                            }
                        }
                        else
                        {
                            if (dialogueUI.alpha <= 0.8f)
                            {
                                SceneManager.LoadScene(dialogues[currentDialogueIndex - 1].sceneToLoad);
                            }
                        }
                        
                    }
                }
                else
                {
                    if (dialogues[currentDialogueIndex].shouldPlayCutscene)
                    {
                        if (dialogueUI.alpha <= 0.8f)
                        {
                            if (dialogues[currentDialogueIndex].shouldContinueCutscene)
                            {
                                cutsceneDirector.Resume();
                            }
                            else
                            {
                                cutsceneDirector.Play();
                            }
                        }
                    }

                    if (dialogues[currentDialogueIndex].shouldLoadScene)
                    {
                        if (dialogues[currentDialogueIndex].dialogueLines.Length > 0)
                        {
                            if (currentInnerDialogueIndex >= dialogues[currentDialogueIndex].dialogueLines.Length - 1)
                            {
                                if (dialogueUI.alpha <= 0.8f)
                                {
                                    SceneManager.LoadScene(dialogues[currentDialogueIndex].sceneToLoad);
                                }
                            }
                        }
                        else
                        {
                            if (dialogueUI.alpha <= 0.8f)
                            {
                                SceneManager.LoadScene(dialogues[currentDialogueIndex].sceneToLoad);
                            }
                        }
                    }
                }
                
               
                if (dialogueUI.alpha == 0f)
                {
                    fadeOut = false;
                    dialogueUI.gameObject.SetActive(false);
                }
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
        if(dialogues[0].shouldUseFadeIn)
        {
            FadeIn();
        }
        else
        {
            dialogueUI.alpha = 1f; // Ensure the dialogue UI is fully visible
        }
        dialogueUI.gameObject.SetActive(true);

        if (dialogues[currentDialogueIndex].isCutSceneDialogue == false)
        {
            speakerNameText.text = dialogues[currentDialogueIndex].speakerData.characterName;
        }
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
            //    dialogueUI.gameObject.SetActive(false);
            //     if (dialogues[currentDialogueIndex -1].isCutSceneDialogue == false)
            //     {
            //         PlayerController.instance.isInteracting = false;
            //     }
            CloseDialogue();
            return; // Important: exit the method here
        }

        // Show the next dialogue
        ShowDialogue();
    }

    public void CloseDialogue()
    {
        if (!dialogues[currentDialogueIndex - 1].isCutSceneDialogue)
        {
            PlayerController.instance.isInteracting = false;
            if (dialogues[currentDialogueIndex - 1].shouldFollowPlayerAfterDialogue)
            {
                PlayerController.instance.talkingWithNPC.StartFollowingPlayer();
                PlayerController.instance.talkingWithNPC = null;
            }
            dialogueUI.gameObject.SetActive(false);
        }
        else
        {
            if (dialogues[currentDialogueIndex - 1].shouldUseFadeOut)
            {
                FadeOut();
            }
            else if (dialogues[currentDialogueIndex - 1].shouldLoadScene)
            {
                SceneManager.LoadScene(dialogues[currentDialogueIndex - 1].sceneToLoad);
            }
        }
    }
}
