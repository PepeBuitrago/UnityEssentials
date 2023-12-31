using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesController : MonoBehaviour
{
    [SerializeField]
    private enum Tipes { Stone, Wood, Straw };

    [SerializeField]
    private Tipes tipe;

    [SerializeField]
    private Item Stone, Wood, Straw;

    [SerializeField]
    private float timeDestroy = 60f;

    [SerializeField]
    private int durability;

    [SerializeField] private Texture2D cursorArrow, cursorPickup; // Asigna el nuevo sprite del cursor desde el editor

    private void Start()
    {
        //durability = Random.Range(1, 4);
        durability = 1;
        StartCoroutine(DestroyDelay());
        transform.Rotate(0, Random.Range(0, 90f), 0);
    }

    

    public void Pickup()
    {
        for(int x = 0; x < durability; x++)
        {
            if(tipe == Tipes.Stone)
            {
                ItemClass itm = Stone.CreateInstance();
                itm.quality = Random.Range(1, 5);
                InventoryManager.instance.Add(itm);
            }
            if (tipe == Tipes.Wood)
            {
                ItemClass itm = Wood.CreateInstance();
                itm.quality = Random.Range(1, 5);
                InventoryManager.instance.Add(itm);
            }
            if (tipe == Tipes.Straw)
            {
                ItemClass itm = Straw.CreateInstance();
                itm.quality = Random.Range(1, 5);
                InventoryManager.instance.Add(itm);
            }
        }
        if (tipe == Tipes.Stone)
        {
            NotificationManager.Instance.Notification("Recogiste " + durability + " de piedra.", 5f);
        }
        if (tipe == Tipes.Wood)
        {
            NotificationManager.Instance.Notification("Recogiste " + durability + " de madera.", 5f);
        }
        if (tipe == Tipes.Straw)
        {
            NotificationManager.Instance.Notification("Recogiste " + durability + " de paja.", 5f);
        }
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);
        Destroy(gameObject);
    }

    void OnMouseEnter()
    {
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if (!isOverUI)
            Cursor.SetCursor(cursorPickup, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(timeDestroy);
        Destroy(gameObject);
    }
}
