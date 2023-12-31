using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    [SerializeField]
    private enum Tipes { Stone };

    [SerializeField]
    private Tipes tipe;

    [SerializeField]
    private string nameStone;


    [SerializeField]
    private int idStone, quality, durability, Minerals;

    [SerializeField]
    private Item Stone, Mineral, Lime;

    [SerializeField] private Texture2D cursorArrow, cursorAxe; // Asigna el nuevo sprite del cursor desde el editor

    public void Mining()
    {
        for (int x = 0; x < durability; x++)
        {
            ItemClass itm = Stone.CreateInstance();
            itm.quality = quality;
            InventoryManager.instance.Add(itm);
            if(Random.Range(0, 25) == 1) 
            {
                ItemClass itm2 = Mineral.CreateInstance();
                itm2.quality = Random.Range(1,101);
                InventoryManager.instance.Add(itm2);
                NotificationManager.Instance.Notification("Has recogido 1 de mineral de hierro.", 5f);
            }
            if (Random.Range(0, 25) == 1)
            {
                ItemClass itm3 = Lime.CreateInstance();
                itm3.quality = Random.Range(1, 101);
                InventoryManager.instance.Add(itm3);
                NotificationManager.Instance.Notification("Has recogido 1 de Cal.", 5f);
            }
        }
        NotificationManager.Instance.Notification("Has recogido " + durability + " de piedra.", 5f);
        InventoryManager.instance.ListItems();
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);
        Destroy(gameObject);

    }

    public string GetNameStone()
    {
        return nameStone;
    }

    public int GetQualityStone()
    {
        return quality;
    }

    public int GetDurabilityStone()
    {
        return durability;
    }

    void OnMouseEnter()
    {
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if(!isOverUI)
            Cursor.SetCursor(cursorAxe, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);
    }
}
