using UnityEngine;

public class Plant : MonoBehaviour
{
    public static Plant instance;

    [Header("Growth Settings")]
    public Sprite Planted;
    public Sprite Watered;
    public int growthStagesCount = 2;
    public GameObject droppedItemPrefab;

    private int currentStage = 0;
    public bool isWatered = false;
    private int lastHarvestedDay = -1;
    private SpriteRenderer spriteRenderer;
    public bool playedWaterSound = false;

    public int CurrentStage => currentStage;
    public bool IsWatered => isWatered;

    private bool shouldBeRemoved = false;

    public void Initialize(Item seedItem)
    {
        Initialize(0, false, GameData.instance.day);
    }

    public void Initialize(int stage, bool watered, int lastHarvestedDay)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (Planted == null || Watered == null)
        {
            Debug.LogError("Growth stages sprites are not assigned.");
            return;
        }

        currentStage = stage;
        isWatered = watered;
        this.lastHarvestedDay = lastHarvestedDay;
        UpdateSprite();

        if (DayManager.instance != null)
        {
            DayManager.instance.RegisterPlant(this);
        }

        Debug.Log($"Plant initialized at stage {currentStage}, watered: {isWatered}, last harvested day: {lastHarvestedDay}.");
    }

    private void OnDestroy()
    {
        if (DayManager.instance != null)
        {
            DayManager.instance.UnregisterPlant(this);
        }
    }

    public void WaterPlant()
    {
        if (currentStage == 0 && !isWatered)
        {
            isWatered = true;
            UpdateSprite();
        }
        else
        {
            Debug.LogWarning("Plant is already watered or not in the correct stage.");
        }
    }

    public void OnDayPass()
    {
        if (isWatered && lastHarvestedDay != GameData.instance.day)
        {
            DropItem();
        }
        else if (!isWatered && lastHarvestedDay != GameData.instance.day)
        {
            Destroy(gameObject);
        }
        lastHarvestedDay = GameData.instance.day;
    }

    private void DropItem()
    {
        if (droppedItemPrefab == null)
        {
            return;
        }

        Instantiate(droppedItemPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;

        if (currentStage == 0)
        {
            spriteRenderer.sprite = isWatered ? Watered : Planted;
        }
        else if (currentStage == 1)
        {
            spriteRenderer.sprite = Watered;
        }
    }

    public void MarkForRemoval()
    {
        shouldBeRemoved = true;
    }

    public bool ShouldBeRemoved()
    {
        return shouldBeRemoved;
    }
}
