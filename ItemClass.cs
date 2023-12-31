using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemClass
{
    public int id;
    public string itemName;
    public float weight;
    public int quality;
    public float durability;
    public string description;
    public bool stackable;

    public ItemClass(Item itemSO)
    {
        id = itemSO.id;
        itemName = itemSO.itemName;
        weight = itemSO.weight;
        quality = itemSO.quality;
        durability = itemSO.durability;
        description = itemSO.description;
        stackable = itemSO.stackable;
    }

    public ItemClass()
    {

    }
}
