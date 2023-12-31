using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    [SerializeField]
    private enum Tipes { Rough, Fruit, Little, Dead, Stump};

    [SerializeField]
    private Tipes tipe;

    [SerializeField]
    private string nameTree;

    
    [SerializeField]
    private int idTree, quality, durability, fruits;

    [SerializeField]
    private GameObject[] NormalTrees;

    [SerializeField]
    private GameObject[] GrowTrees;

    [SerializeField]
    private GameObject[] FruitTrees;

    [SerializeField]
    private GameObject[] StumpTrees;

    [SerializeField]
    private Item Wood;

    [SerializeField] private Texture2D cursorArrow, cursorAxe; // Asigna el nuevo sprite del cursor desde el editor

    private void Start()
    {
        fruits = Random.Range(0, 10);
    }

    public void Cut()
    {
        for (int x = 0; x < durability; x++)
        {
            ItemClass itm = Wood.CreateInstance();
            itm.quality = quality;
            InventoryManager.instance.Add(itm);
        }
        NotificationManager.Instance.Notification("Has recogido " + durability + " de madera.", 5f);
        InventoryManager.instance.ListItems();

        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);

        if (tipe != Tipes.Stump)
        {
            if (tipe == Tipes.Dead)
            {
                GameObject newTree = Instantiate(StumpTrees[0], transform.position, Quaternion.identity, transform.parent);
            }
            if (tipe == Tipes.Rough)
            {
                GameObject newTree = Instantiate(StumpTrees[1], transform.position, Quaternion.identity, transform.parent);
            }
            if (tipe == Tipes.Fruit || tipe == Tipes.Little)
            {
                GameObject newTree = Instantiate(StumpTrees[2], transform.position, Quaternion.identity, transform.parent);
            }
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GetNameTree()
    {
        return nameTree;
    }
    public int GetQualityTree()
    {
        return quality;
    }
    public int GetDurabilityTree()
    {
        return durability;
    }


    void OnMouseEnter()
    {
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if (!isOverUI)
            Cursor.SetCursor(cursorAxe, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);
    }
}
