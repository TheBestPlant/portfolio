using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> items = new List<Item>();

    public Item GetItemByID(int id)
    {
        return items.Find(item => item.ID == id);
    }

    public Item GetItemByName(string itemName)
    {
        return items.Find(item => item.name == itemName);
    }
}
