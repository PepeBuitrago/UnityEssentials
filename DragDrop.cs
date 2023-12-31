using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public ItemClass itemData; // Scriptable Object asociado al ítem

    public void OnDrag(PointerEventData eventData)
    {
        // Lógica para arrastrar el ítem
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Lógica al soltar el ítem
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            
            DragDrop otherItem = hit.collider.GetComponent<DragDrop>();

            // Verificar si los dos ítems son del mismo tipo
            if (otherItem != null)
            {
                // Fusionar los ítems y actualizar la cantidad
                //otherItem.itemData.quantity += itemData.quantity;
                Destroy(gameObject); // Eliminar el ítem arrastrado
            }
        }
    }
}
