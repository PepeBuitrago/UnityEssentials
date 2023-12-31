using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public ItemClass itemData; // Scriptable Object asociado al �tem

    public void OnDrag(PointerEventData eventData)
    {
        // L�gica para arrastrar el �tem
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // L�gica al soltar el �tem
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            
            DragDrop otherItem = hit.collider.GetComponent<DragDrop>();

            // Verificar si los dos �tems son del mismo tipo
            if (otherItem != null)
            {
                // Fusionar los �tems y actualizar la cantidad
                //otherItem.itemData.quantity += itemData.quantity;
                Destroy(gameObject); // Eliminar el �tem arrastrado
            }
        }
    }
}
