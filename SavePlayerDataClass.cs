using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public class SavePlayerDataClass
{
    public string name;
    public float healt;
    public float stamine;
    public float hungry;
    public float nvl;

    public float coins;

    public int gender;
    public int hair;
    public int facialHair;
    public int colorHair;
    public int skin;

    public bool ban;
    public int admin;

    public float coordX;
    public float coordY;
    public float coordZ;

    public string inventoryData;

    //public SaveDataInventory inventoryData = new SaveDataInventory(); // Clase contenedora de la lista

    public SavePlayerDataClass()
    {

    }
}

[System.Serializable]
public class SaveDataInventory
{
    public List<ItemClass> inventory = new List<ItemClass>();
}