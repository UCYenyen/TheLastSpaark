using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public CharacterSO characterData;
    public bool canInteractWithPlayer = false;
    public bool isInInteractArea = false;
    public GameObject npcNameGameObject;
    public TextMeshProUGUI npcNameText;
    public GameObject pressEToInteractGameObject;
    public DialogueSO[] dialogueData;

    public bool shouldFollowPlayer = false;
    bool isFollowingPlayer = false;
    bool isInDialogue = false;
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
    }
    public void Interact()
    {
        pressEToInteractGameObject.SetActive(false);
        PlayerController.instance.isInteracting = true;
        PlayerController.instance.ResetVelocity();
        PlayerController.instance.talkingWithNPC = this;
        UpdateNPCName();
        DialogueController.instance.SetDialogues(dialogueData);
        DialogueController.instance.ShowDialogue();
    }

    void FixedUpdate()
    {
        if (isFollowingPlayer)
        {
            Vector2 playerPosition = PlayerController.instance.transform.position;
            Vector2 npcPosition = transform.position;

            if (Vector2.Distance(playerPosition, npcPosition) > 0.5f)
            {
                transform.position = Vector2.MoveTowards(npcPosition, playerPosition, 2.25f * Time.fixedDeltaTime);
            }
        }
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
