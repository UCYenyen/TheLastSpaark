using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    public Item[] ownedItems = new Item[3];

    [Header("Item Slots")]
    public Image[] itemsSlots = new Image[3];
    public Sprite emptySlotSprite;
    void Start()
    {
        UpdateInventoryUI();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < itemsSlots.Length; i++)
        {
            if (ownedItems[i] != null)
            {
                itemsSlots[i].sprite = ownedItems[i].itemIcon;
                itemsSlots[i].color = Color.white;
                itemsSlots[i].SetNativeSize();
            }
            else
            {
                itemsSlots[i].sprite = emptySlotSprite;
                itemsSlots[i].color = Color.clear;
                itemsSlots[i].SetNativeSize();
            }
        }
    }
    public bool CheckIfItemExists(string itemName)
    {
        for (int i = 0; i < ownedItems.Length; i++)
        {
            if (ownedItems[i].itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }
    public void UseItem(int index)
    {
        if (index < 0 || index >= ownedItems.Length || ownedItems[index] == null)
        {
            return;
        }

        if (ownedItems[index].isConsumable)
        {
            // Apply item effects here
            UIController.instance.itemNotificationText.text = "Menggunakan " + ownedItems[index].itemName;
            StartCoroutine(hideNotificationText());

            PlayerController.instance.healthController.Heal(ownedItems[index].healthRestoreAmount);
            removeItem(ownedItems[index].itemName);
        }
        else
        {
            UIController.instance.itemNotificationText.text = "Item ini tidak dapat digunakan!";
            StartCoroutine(hideNotificationText());
        }
    }
    IEnumerator hideNotificationText()
    {
        UIController.instance.itemNotificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        UIController.instance.itemNotificationText.gameObject.SetActive(false);
    }
    public void removeItem(string itemName)
    {
        for (int i = 0; i < ownedItems.Length; i++)
        {
            if (ownedItems[i].itemName == itemName)
            {
                ownedItems[i] = null;
                UpdateInventoryUI();
                break;
            }
        }
    }
    public void AddItem(Item itemToAdd)
    {
        for (int i = 0; i < ownedItems.Length; i++)
        {
            if (ownedItems[i] == null)
            {
                ownedItems[i] = itemToAdd;
                UpdateInventoryUI();
                return;
            }
        }
    }
}
