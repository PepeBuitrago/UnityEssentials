using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DropedItemController : NetworkBehaviour
{
    [SerializeField] private List<ItemClass> chest = new List<ItemClass>();
    [SerializeField] private Texture2D cursorArrow, cursorItem; // Asigna el nuevo sprite del cursor desde el editor

    public void Add(ItemClass item)
    {
        chest.Add(item);
    }
    public void Remove(ItemClass item)
    {
        chest.Remove(item);
    }

    


    public void Pickup(out List<ItemClass> itemsDropped)
    {
        itemsDropped = chest;
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);

        CmdDestroyItem();
    }

    [Command]
    public void CmdDestroyItem()
    {
        NetworkServer.UnSpawn(gameObject);
        Destroy(gameObject);
    }

    void OnMouseEnter()
    {
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if (!isOverUI)
            Cursor.SetCursor(cursorItem, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);
    }

}
