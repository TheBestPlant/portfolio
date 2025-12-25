using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Deselect();
    }

    public void Select()
    {
        if (image != null)
        {
            image.color = selectedColor;
        }
        else
        {
            Debug.LogWarning("Image component is not assigned on " + gameObject.name);
        }
    }

    public void Deselect()
    {
        if (image != null)
        {
            image.color = notSelectedColor;
        }
        else
        {
            Debug.LogWarning("Image component is not assigned on " + gameObject.name);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (inventoryItem != null)
            {
                inventoryItem.parentAfterDrag = transform;
            }
        }
    }
}

