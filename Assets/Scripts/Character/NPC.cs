using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;
    Vector2 moveDir;

    [Header("NPC Settings")]
    public CharacterSO characterData;
    public bool canInteractWithPlayer = false;
    public bool isInInteractArea = false;
    public bool shouldFollowPlayer = false;
    [HideInInspector] public bool isFollowingPlayer = false;

    [Header("Follow Other NPC Settings")]
    public bool shouldFollowOtherNPC = false;
    public bool canFollowOtherNPC = false;
    public bool isFollowingOtherNPC = false;
    public NPC otherNPCToFollow;

    [Header("Freeze Settings")]
    public Image freezeMeterFillAmount;
    public Animator freezeAnim;
    public bool isNearLightSource = false;
    public bool canBeFrozen = false;
    public bool isFrozen = false;
    public float freezeRate = 1f; // Rate at which the freeze meter increases
    public float maxFreezeMeter = 100f;
    public float currentFreezeMeter;
    public FreezeCrystal freezeCrystal;
    
    [Header("UI")]
    public GameObject npcNameGameObject;
    public TextMeshProUGUI npcNameText;
    public GameObject pressEToInteractGameObject;
    public DialogueSO[] dialogueData;

    [Header("Quest Settings")]
    public bool isQuestNPC = false;
    public QuestSO questData;

    [Header("Dialogue Changer")]
    public DialogueChanger dialogueChanger;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canInteractWithPlayer)
            {
                isInInteractArea = true;
                npcNameGameObject.SetActive(true);
                pressEToInteractGameObject.SetActive(true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInInteractArea = false;
            npcNameGameObject.SetActive(false);
            pressEToInteractGameObject.SetActive(false);
        }
    }
    void Update()
    {
        CheckWhichQuestToComplete();
        freezeMeterFillAmount.fillAmount = currentFreezeMeter / maxFreezeMeter;

        if (!isFrozen)
        {
            LookAtPlayer();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInInteractArea && canInteractWithPlayer)
            {
                if (pressEToInteractGameObject.activeSelf)
                {
                    if (!PlayerController.instance.isInteracting)
                    {
                        Interact();
                    }
                }
            }
        }

        if (canBeFrozen)
        {
            if (!isNearLightSource)
            {
                calculateTimeToFreeze();
            }
            else
            {
                if (currentFreezeMeter > 0)
                {
                    currentFreezeMeter -= freezeRate * Time.deltaTime;
                }
            }
            
        }
    }
    void LookAtPlayer()
    {
        moveDir = PlayerController.instance.transform.position - transform.position;
        moveDir.Normalize();

        anim.SetFloat("dirX", moveDir.x);
        anim.SetFloat("dirY", moveDir.y);
    }
    public void Interact()
    {
        pressEToInteractGameObject.SetActive(false);
        PlayerController.instance.isInteracting = true;
        PlayerController.instance.ResetVelocity();

        if (isFrozen)
        {
            MeltFrozenCrsystal();
        }
        else
        {
            PlayerController.instance.talkingWithNPC = this;
            UpdateNPCName();
            DialogueController.instance.SetDialogues(dialogueData);
            DialogueController.instance.ShowDialogue();
        }
    }
    void CheckWhichQuestToComplete()
    {
        if (characterData.characterName == "Liora")
        {
            CheckLioraQuest();
        }
        if (characterData.characterName == "Vera")
        {
            CheckVeraQuest();
        }
    }
    void CheckLioraQuest()
    {
        if (isQuestNPC && questData != null)
        {
            if (QuestController.instance.activeQuests.Contains(questData) && questData.isQuestCompleted == false)
            {
                // Logic to handle quest interaction
                if (PlayerController.instance.currentRoom != null)
                {
                     if (PlayerController.instance.currentRoom.roomName == questData.destination)
                    {
                        if (Vector2.Distance(PlayerController.instance.transform.position, transform.position) <= 1f)
                        {
                            QuestController.instance.CompleteQuest(questData);
                            isFollowingPlayer = false;
                            shouldFollowPlayer = false;
                            anim.SetBool("isWalking", false);

                            if (dialogueChanger != null)
                            {
                                dialogueChanger.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }

    void CheckVeraQuest()
    {
        if (isQuestNPC && questData != null)
        {
            if (QuestController.instance.activeQuests.Contains(questData) && questData.isQuestCompleted == false)
            {
                // Logic to handle quest interaction
                if (PlayerController.instance.currentRoom != null)
                {
                     if (PlayerController.instance.currentRoom.roomName == questData.destination)
                    {
                        NPC tamon = GameManager.instance.FindNPC("Tamon");
                        if (Vector2.Distance(transform.position, tamon.transform.position) <= 1f)
                        {
                            QuestController.instance.CompleteQuest(questData);
                            tamon.isFollowingPlayer = false;
                            tamon.shouldFollowPlayer = false;
                            tamon.anim.SetBool("isWalking", false);

                            if (dialogueChanger != null)
                            {
                                dialogueChanger.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isFollowingPlayer && isFrozen == false)
        {
            Vector2 playerPosition = PlayerController.instance.transform.position;
            Vector2 npcPosition = transform.position;

            if (Vector2.Distance(playerPosition, npcPosition) > 1f)
            {
                if (Vector2.Distance(playerPosition, npcPosition) < 5f)
                {
                    transform.position = Vector2.MoveTowards(npcPosition, playerPosition, 2.25f * Time.fixedDeltaTime);
                    anim.SetBool("isWalking", true);
                }
                else
                {
                    anim.SetBool("isWalking", false);
                }
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
        else if (isFollowingOtherNPC && isFrozen == false)
        {
            if (otherNPCToFollow != null)
            {
                Vector2 position = transform.position;
                Vector2 targetPosition = otherNPCToFollow.transform.position;
                if (Vector2.Distance(position, targetPosition) > 2f)
                {
                    if (Vector2.Distance(position, targetPosition) < 5f)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 2.25f * Time.fixedDeltaTime);
                        anim.SetBool("isWalking", true);
                    }
                    else
                    {
                        anim.SetBool("isWalking", false);
                    }
                }
                else
                {
                    anim.SetBool("isWalking", false);
                }
            }
        }
    }
    private void calculateTimeToFreeze()
    {
        if (canBeFrozen)
        {
            if (!isFrozen)
            {
                if (currentFreezeMeter < maxFreezeMeter)
                {
                    currentFreezeMeter += Time.deltaTime * freezeRate; // Increase freeze meter over time
                }

                if (currentFreezeMeter >= maxFreezeMeter)
                {
                    if (freezeCrystal != null)
                    {
                        freezeCrystal.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    public void MeltFrozenCrsystal()
    {
        pressEToInteractGameObject.SetActive(true);
        PlayerController.instance.isInteracting = true;
        freezeCrystal.DisableCrystal();
        PlayerController.instance.playerTorch.currentTorchLight = 0;
    }

    public void StopFollowingPlayer()
    {
        isFollowingPlayer = false;
    }
    public void StartFollowingPlayer()
    {
        isFollowingPlayer = true;
    }
    public void UpdateNPCName()
    {
        if (npcNameText != null)
        {
            npcNameText.text = characterData.characterName;
        }
    }
}
