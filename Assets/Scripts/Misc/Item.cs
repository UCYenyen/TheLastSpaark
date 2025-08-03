using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public bool isConsumable = false;

    [Header("Item Effects")]
    public float healthRestoreAmount = 0f;
    public float manaRestoreAmount = 0f;
    public float staminaRestoreAmount = 0f;
}
