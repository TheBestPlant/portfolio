using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;

    public Text countText;

    [Header("UI")]
    public Image image;
    public Color selectedColor, notSelectedColor;

    [HideInInspector] public Transform parentAfterDrag;

    public void InitializeItem(Item newItem)
    {
        if (newItem != null)
        {
            item = newItem;
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            image.sprite = newItem.image;
            RefreshCount();
        }
        else
        {
            Debug.LogWarning("Attempted to initialize with a null item.");
        }
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (image == null)
        {
            image = GetComponent<Image>();
            if (image == null)
            {
                Debug.LogError("Image component is not assigned and not found on the GameObject.");
            }
        }

        if (item != null)
        {
            InitializeItem(item);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (image != null)
        {
            if (countText != null)
            {
                countText.raycastTarget = false;
            }
            image.raycastTarget = false;
        }
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (image != null)
        {
            image.raycastTarget = true;
        }
        transform.SetParent(parentAfterDrag);
        if (countText != null)
        {
            countText.raycastTarget = true;
        }
    }

    public void Deselect()
    {
        if (image != null)
        {
            image.color = notSelectedColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item.actionType == ActionType.Sell)
            {
                Customer customer = FindObjectOfType<Customer>();
                if (customer != null)
                {
                    customer.SellItem();
                }
            }
            else if (item.actionType == ActionType.Plant)
            {
                PlantManager.instance.PlantSeed(item);
            }
        }
    }
}
