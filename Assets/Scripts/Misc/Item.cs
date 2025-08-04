using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Pickup Settings")]
    public bool canBePickedUp = false;
    public bool isQuestItem = false;
    public bool isInRange = false;
    public Sprite inRangeSprite;
    public Sprite outOfRangeSprite;
    public SpriteRenderer itemSpriteRenderer;

    [Header("Item Settings")]
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public bool isConsumable = false;

    [Header("Item Effects")]
    public float healthRestoreAmount = 0f;
    public float manaRestoreAmount = 0f;
    public float staminaRestoreAmount = 0f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            // Change the item sprite to indicate it's in range
            itemSpriteRenderer.sprite = inRangeSprite;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            // Change the item sprite back to indicate it's out of range
            itemSpriteRenderer.sprite = outOfRangeSprite;
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }
    public void PickupItem()
    {
        if (canBePickedUp)
        {
            UIController.instance.inventory.AddItem(this);
            this.gameObject.SetActive(false);
        }
    }
}
