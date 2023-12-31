using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public float weight;
    public int quality;
    public float durability;
    public string description;
    public bool stackable;



    public ItemClass CreateInstance()
    {
        ItemClass newItem = new ItemClass(this);
        return newItem;
    }
}
