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
    bool isFollowingPlayer = false;

    [Header("Freeze Settings")]
    public Image freezeMeterFillAmount;
    public Animator freezeAnim;
    public bool isNearLightSource = false;
    public bool canBeFrozen = false;
    public bool isFrozen = false;
    public float freezeRate = 1f; // Rate at which the freeze meter increases
    public float maxFreezeMeter = 100f;
    [HideInInspector] public float currentFreezeMeter;
    public FreezeCrystal freezeCrystal;
    
    [Header("UI")]
    public GameObject npcNameGameObject;
    public TextMeshProUGUI npcNameText;
    public GameObject pressEToInteractGameObject;
    public DialogueSO[] dialogueData;

    [Header("Quest Settings")]
    public bool isQuestNPC = false;
    public QuestSO questData;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInInteractArea = true;
            npcNameGameObject.SetActive(true);
            pressEToInteractGameObject.SetActive(true);
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

    void FixedUpdate()
    {
        if (isFollowingPlayer && isFrozen == false)
        {
            Vector2 playerPosition = PlayerController.instance.transform.position;
            Vector2 npcPosition = transform.position;

            if (Vector2.Distance(playerPosition, npcPosition) > 1f)
            {
                transform.position = Vector2.MoveTowards(npcPosition, playerPosition, 2.25f * Time.fixedDeltaTime);
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
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
