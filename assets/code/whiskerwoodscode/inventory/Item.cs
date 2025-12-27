using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    public ItemType type;
    public ActionType actionType;
    public int cost;
    public Item harvestItem;

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
    public int ID;

    [Header("Dropped Item")]
    public GameObject droppedItemPrefab;
}

public enum ItemType
{
    Seed,
    Food
}

public enum ActionType
{
    Plant,
    Sell
}
