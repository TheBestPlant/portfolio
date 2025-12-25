using UnityEngine;
using UnityEngine.EventSystems;

public class Customer : MonoBehaviour, IPointerClickHandler
{
    public Item itemToBuy;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            SellItem();
        }
    }

    public void SellItem()
    {
        if (InventoryItemsManager.instance == null)
        {
            Debug.LogError("InventoryItemsManager instance is missing.");
            return;
        }

        Item selectedItem = InventoryItemsManager.instance.GetSelectedItem(false);

        if (selectedItem == null)
        {
            return;
        }

        if (selectedItem == itemToBuy)
        {
            int price = InventoryItemsManager.instance.GetSelectedItem(true).cost;
            UIScript.instance.AddPoints(0, price);
        }
        else
        {
            Debug.Log("The selected item is not what the customer wants.");
        }
    }
}
