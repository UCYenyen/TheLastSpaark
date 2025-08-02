using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public CharacterSO characterData;
    public bool canInteractWithPlayer = false;
    public bool isInInteractArea = false;
    public GameObject npcNameGameObject;
    public GameObject pressEToInteractGameObject;
    public DialogueSO[] dialogueData;
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
        DialogueController.instance.SetDialogues(dialogueData);
        DialogueController.instance.ShowDialogue();
    }
}
