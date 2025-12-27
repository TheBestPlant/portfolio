using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private Item item;

    private void Start()
    {
        if (DayManager.instance != null)
        {
            DayManager.instance.RegisterDroppedItem(this);
        }
    }

    private void OnDestroy()
    {
        if (DayManager.instance != null)
        {
            DayManager.instance.UnregisterDroppedItem(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventoryItemsManager.instance == null || item == null)
            {
                return;
            }

            bool canAdd = InventoryItemsManager.instance.AddItem(item);
            if (canAdd)
            {
                GameObject uiItemGO = CreateUIItem();
                StartCoroutine(MoveAndCollect(other.transform, uiItemGO));
            }
            else
            {
                Debug.LogWarning("Failed to add item to inventory.");
            }
        }
    }

    private GameObject CreateUIItem()
    {
        GameObject uiItemGO = Instantiate(UIManager.instance.inventoryItemPrefab, transform.position, Quaternion.identity);
        return uiItemGO;
    }

    private IEnumerator MoveAndCollect(Transform target, GameObject uiItemGO)
    {
        Collider2D coll = GetComponent<Collider2D>();
        if (coll != null)
        {
            coll.enabled = false;
        }

        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 5f);
            if (uiItemGO != null)
            {
                uiItemGO.transform.position = transform.position;
            }
            yield return null;
        }

        Destroy(uiItemGO);
        Destroy(gameObject);
    }

    public void SetDrop(Item newItem)
    {
        item = newItem;
        SaveItemState();
    }

    public void SaveItemState()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string prefabName = item.droppedItemPrefab.name;
        string key = $"DroppedItem_{sceneName}_{prefabName}_{transform.position.x}_{transform.position.y}";

        PlayerPrefs.SetString($"{key}_ItemID", item.ID.ToString());
        PlayerPrefs.SetString($"{key}_SceneName", sceneName);
        PlayerPrefs.SetString($"{key}_PrefabName", prefabName);
        PlayerPrefs.SetFloat($"{key}_PosX", transform.position.x);
        PlayerPrefs.SetFloat($"{key}_PosY", transform.position.y);
        Debug.Log($"Saved item {item.ID.ToString()} in {sceneName} at ({transform.position.x}, {transform.position.y}) with prefab {prefabName}");
    }


    public static void SaveAllDroppedItems()
    {
        string allKeys = "";
        foreach (var item in FindObjectsOfType<DroppedItem>())
        {
            string prefabName = item.gameObject.name;
            string key = $"DroppedItem_{SceneManager.GetActiveScene().name}_{prefabName}_{item.transform.position.x}_{item.transform.position.y}";
            allKeys += key + ";";
            item.SaveItemState();
        }
        PlayerPrefs.SetString("AllDroppedItemKeys", allKeys);
        PlayerPrefs.Save();
    }

    public static void LoadAllDroppedItems(Transform parent)
    {
        string allKeysString = PlayerPrefs.GetString("AllDroppedItemKeys", "");
        if (string.IsNullOrEmpty(allKeysString))
        {
            Debug.Log("No dropped item keys found in PlayerPrefs.");
            return;
        }

        string[] keys = allKeysString.Split(';');

        foreach (string key in keys)
        {
            if (!string.IsNullOrEmpty(key))
            {
                LoadDroppedItem(key, parent);
            }
        }
    }

    public static DroppedItem LoadDroppedItem(string key, Transform parent)
    {

        if (!PlayerPrefs.HasKey($"{key}_ItemID"))
        {
            Debug.LogWarning($"No item ID found for key: {key}");
            return null;
        }

        string sceneName = PlayerPrefs.GetString($"{key}_SceneName");
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            Debug.LogWarning($"Current scene ({SceneManager.GetActiveScene().name}) does not match saved scene ({sceneName}) for key: {key}");
            return null;
        }

        int itemID = int.Parse(PlayerPrefs.GetString($"{key}_ItemID"));
        float posX = PlayerPrefs.GetFloat($"{key}_PosX");
        float posY = PlayerPrefs.GetFloat($"{key}_PosY");
        string prefabName = PlayerPrefs.GetString($"{key}_PrefabName");

        Item item = ItemDatabaseManager.instance.itemDatabase.GetItemByID(itemID);
        if (item == null)
        {
            Debug.LogWarning($"No item found with ID: {itemID}");
            return null;
        }

        GameObject droppedItemPrefab = Resources.Load<GameObject>(prefabName);
        if (droppedItemPrefab == null)
        {
            Debug.LogError($"Prefab '{prefabName}' not found in Resources folder.");
            return null;
        }

        GameObject droppedItemGO = Instantiate(droppedItemPrefab, new Vector3(posX, posY, 0), Quaternion.identity, parent);
        if (droppedItemGO != null)
        {
            Debug.Log($"Prefab instantiated: {droppedItemGO.name} at ({posX}, {posY})");
        }
        else
        {
            Debug.LogError("Failed to instantiate prefab.");
        }

        DroppedItem droppedItem = droppedItemGO.GetComponent<DroppedItem>();
        droppedItem.SetDrop(item);
        return droppedItem;
    }

}
