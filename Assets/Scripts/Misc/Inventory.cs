using UnityEngine.UI;
using UnityEngine;

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
            }
            else
            {
                itemsSlots[i].sprite = emptySlotSprite;
                itemsSlots[i].color = Color.clear;
            }
        }
    }
    public void UseItem(int index)
    {
        if (index < 0 || index >= ownedItems.Length || ownedItems[index] == null)
        {
            Debug.Log("Invalid item index or item does not exist.");
            return;
        }

        if (ownedItems[index].isConsumable)
        {
            // Apply item effects here
            
        }
    }
    public void removeItem(Item itemToRemove)
    {
        for (int i = 0; i < ownedItems.Length; i++)
        {
            if (ownedItems[i] == itemToRemove)
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
        Debug.Log("Inventory is full!");
    }
}
