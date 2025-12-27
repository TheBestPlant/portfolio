using UnityEngine;

public class InventoryItemsManager : MonoBehaviour
{
    public static InventoryItemsManager instance;

    public int maxStackedItems = 100;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public AudioSource audioSource;
    public AudioClip slotSwitchSound;

    int selectedSlot = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSelectedSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSelectedSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSelectedSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSelectedSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeSelectedSlot(4);
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        PlaySlotSwitchSound();
    }

    public bool AddItem(Item item)
    {
        //See if the item is already in the inventory, if it is collect the item and increase its count
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item != null && itemInSlot.item.name == item.name && itemInSlot.count < maxStackedItems && item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                GameData.instance.pickUpItem(item.ID);
                return true;
            }
        }

        //See if there's an empty slot, if there is then collect the item
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                GameData.instance.pickUpItem(item.ID);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        if (selectedSlot < 0 || selectedSlot >= inventorySlots.Length) return null;

        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
                GameData.instance.usedItem(item.ID);
            }
            return item;
        }
        return null;
    }

    private void PlaySlotSwitchSound()
    {
        if (audioSource != null && slotSwitchSound != null)
        {
            audioSource.PlayOneShot(slotSwitchSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or SlotSwitchSound is not assigned.");
        }
    }
}
