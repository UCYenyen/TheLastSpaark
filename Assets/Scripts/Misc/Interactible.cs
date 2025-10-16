using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactible : MonoBehaviour
{
    [Header("Animations")]
    public bool shouldFadeOutOnInteract = false;
    public bool shouldFadeInOnInteract = false;
    public bool shouldFadeInOut = false;

    [Header("ConsumeTorchToInteract")]
    public bool shouldConsumeTorchToInteract = false;

    [Header("Interactible Settings")]
    public bool requiresSpesificQuestToInteract = false;
    public QuestSO spesificQuestToInteract;

    [Header("Should Change Location After Interact")]
    public bool shouldChangeLocationAfterInteract = false;
    public Transform newLocation;

    [Header("Dialogue Settings")]
    public bool shouldShowDialogueOnInteract = false;
    public DialogueSO[] dialogueToShow;

    [Header("Should Show Cutscene")]
    public bool shouldShowCutsceneOnInteract = false;
    public DialogueSO[] cutsceneDialogueToShow;

    [Header("Should Make Gameobject Appear")]
    public bool shouldMakeGameObjectAppear = false;
    public GameObject gameObjectToAppear;

    [Header("Should Deactivate GameObject")]
    public bool shouldDeactivateGameObject = false;
    public GameObject gameObjectToDeactivate;

    [Header("Should change GameObject's Sprite")]
    public bool shouldChangeSprite = false;
    public SpriteRenderer[] spriteRendererToChange;
    public Sprite[] newSprite;

    [Header("Interaction Components")]
    public bool canOnlyInteractOnce = false;
    bool hasInteracted = false;
    public bool shouldUsePressedEToInteract = true;
    public GameObject pressEToInteractGameObject;
    bool isPlayerInRange = false;
    public QuestSO tamonQuest;

    [Header("Should Change Scene On Interact")]
    public bool shouldChangeSceneOnInteract = false;
    public string sceneToLoad;

    void Update()
    {
        if (isPlayerInRange && PlayerController.instance.isInteracting == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && shouldUsePressedEToInteract)
            {
                if (canOnlyInteractOnce == false)
                {
                    Interact();
                }
                else
                {
                    if (hasInteracted == false)
                    {
                        hasInteracted = true;
                        Interact();
                    }
                }
            }

            if (shouldUsePressedEToInteract == false)
            {
                Interact();
            }
        }
    }
    IEnumerator changePosition()
    {
        NPC Tamon = GameManager.instance.FindNPC("Tamon");
        NPC Vera = GameManager.instance.FindNPC("Vera");

        yield return new WaitForSeconds(0.75f);
        PlayerController.instance.transform.position = newLocation.position;

        if (QuestController.instance.activeQuests.Contains(tamonQuest))
        {
            if (tamonQuest.isQuestCompleted == false)
            {
                Tamon.transform.position = newLocation.position;
                Tamon.canInteractWithPlayer = false;
                Tamon.isFollowingPlayer = false;
                Tamon.shouldFollowPlayer = false;
                Tamon.isFollowingOtherNPC = true;
                Tamon.canBeFrozen = false;
                Tamon.canFollowOtherNPC = true;

                Vera.canBeFrozen = false;
            }
        }
    }
    private void Interact()
    {
        PlayerController.instance.ResetVelocity();
        if (shouldConsumeTorchToInteract)
        {
            if (PlayerController.instance.playerTorch.currentTorchLight > 0)
            {
                PlayerController.instance.playerTorch.UseTorch(100);
            }
            else
            {
                return;
            }
        }
        if (shouldFadeOutOnInteract)
        {
            UIController.instance.FadeOut();
        }
        if (shouldFadeInOnInteract)
        {
            UIController.instance.FadeIn();
        }
        if (shouldFadeInOut)
        {
            UIController.instance.InstantFadeInFadeOut();
        }
        if (shouldChangeLocationAfterInteract)
        {
            StartCoroutine(changePosition());
        }
        if (shouldShowDialogueOnInteract)
        {
            DialogueController.instance.SetDialogues(dialogueToShow);
            DialogueController.instance.ShowDialogue();
        }

        if (shouldShowCutsceneOnInteract)
        {
            DialogueController.instance.SetDialogues(cutsceneDialogueToShow);
            DialogueController.instance.ShowDialogue();
        }

        if (shouldChangeSprite)
        {
            for (int i = 0; i < spriteRendererToChange.Length; i++)
            {
                spriteRendererToChange[i].sprite = newSprite[i];
            }
        }

        if (shouldMakeGameObjectAppear)
        {
            gameObjectToAppear.SetActive(true);
        }
        if (shouldDeactivateGameObject)
        {
            gameObjectToDeactivate.SetActive(false);
        }
        if (shouldChangeSceneOnInteract)
        {
            StartCoroutine(ChangeScene());
        }
    }
    IEnumerator ChangeScene()
    {
        UIController.instance.FadeOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (requiresSpesificQuestToInteract == false)
            {
                isPlayerInRange = true;
                pressEToInteractGameObject.SetActive(true);
            }
            else
            {
                if (QuestController.instance.activeQuests.Contains(spesificQuestToInteract))
                {
                    isPlayerInRange = true;
                    pressEToInteractGameObject.SetActive(true);
                }
                else
                {
                    if (spesificQuestToInteract.isQuestCompleted)
                    {
                        isPlayerInRange = true;
                        pressEToInteractGameObject.SetActive(true);
                    }
                    else
                    {
                        isPlayerInRange = false;
                        pressEToInteractGameObject.SetActive(false);
                    }
                    
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (requiresSpesificQuestToInteract == false)
            {
                isPlayerInRange = true;
                pressEToInteractGameObject.SetActive(true);
            }
            else
            {
                
                if (QuestController.instance.activeQuests.Contains(spesificQuestToInteract))
                {
                    isPlayerInRange = true;
                    pressEToInteractGameObject.SetActive(true);
                }
                else
                {
                    if (spesificQuestToInteract.isQuestCompleted)
                    {
                        isPlayerInRange = true;
                        pressEToInteractGameObject.SetActive(true);
                    }
                    else
                    {
                        isPlayerInRange = false;
                        pressEToInteractGameObject.SetActive(false);
                    }
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            pressEToInteractGameObject.SetActive(false);
        }
    }
}
