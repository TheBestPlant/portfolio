using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DayManager : MonoBehaviour
{
    public static DayManager instance;

    [System.Serializable]
    public struct SpawnableItem
    {
        public GameObject itemPrefab;
        [Range(0, 100)]
        public float spawnChance;
    }

    public List<Transform> spawnPoints = new List<Transform>();
    public List<SpawnableItem> spawnableItems = new List<SpawnableItem>();

    private List<Plant> plants = new List<Plant>();
    private List<DroppedItem> droppedItems = new List<DroppedItem>();

    private bool isNewDay = false;
    private int newDayForest = 0;
    private int newDayGreenhouse = 0;

    private List<Plant> plantsToRemove = new List<Plant>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadDroppedItems();
    }

    private void OnApplicationQuit()
    {
        SaveDroppedItems();
    }

    public void RegisterPlant(Plant plant)
    {
        if (!plants.Contains(plant))
        {
            plants.Add(plant);
        }
    }

    public void UnregisterPlant(Plant plant)
    {
        if (plants.Contains(plant))
        {
            plants.Remove(plant);
        }
    }

    public void RegisterDroppedItem(DroppedItem item)
    {
        if (!droppedItems.Contains(item))
        {
            droppedItems.Add(item);
        }
    }

    public void UnregisterDroppedItem(DroppedItem item)
    {
        if (droppedItems.Contains(item))
        {
            droppedItems.Remove(item);
        }
    }

    public void OnDayPass()
    {
        GameData.instance.newDay();
        isNewDay = true;
        HarvestWateredPlants();
    }

    //Spawns and harvests plants in Greenhouse and spawns seeds in forest if it's a new day
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Forest")
        {
            InitializeForestScene();
            if (newDayForest != GameData.instance.day)
            {
                newDayForest = GameData.instance.day;
                SpawnItemsAtLocations();
            }
            DroppedItem.LoadAllDroppedItems(transform);
        }
        else if (scene.name == "Greenhouse")
        {
            if (newDayGreenhouse != GameData.instance.day)
            {
                newDayGreenhouse = GameData.instance.day;
                PlantManager.instance.LoadPlants();
                HarvestWateredPlants();
            }
        }
        CharacterManager.instance.UpdateCharacter(CharacterManager.instance.selectedOption);
    }

    private void InitializeForestScene()
    {
        spawnPoints = new List<Transform>(GameObject.FindGameObjectsWithTag("ForestSpawnPoint").Select(go => go.transform));
    }

    private void SpawnItemsAtLocations()
    {
        int spawnPointsCount = spawnPoints.Count;
        int spawnableItemsCount = spawnableItems.Count;

        int minCount = Mathf.Min(spawnPointsCount, spawnableItemsCount);

        for (int i = 0; i < minCount; i++)
        {
            var spawnableItem = spawnableItems[i];
            var spawnPoint = spawnPoints[i];

            Instantiate(spawnableItem.itemPrefab, spawnPoint.position, Quaternion.identity);
        }
    }



    public void HarvestWateredPlants()
    {
        List<Plant> plantsToHarvest = new List<Plant>();

        foreach (Plant plant in plants)
        {
            if (plant.GetComponent<SpriteRenderer>().sprite == plant.Watered)
            {
                plantsToHarvest.Add(plant);
            }
        }

        foreach (Plant plant in plantsToHarvest)
        {
            plant.OnDayPass();
            plant.MarkForRemoval();
            plant.isWatered = false;
        }
    }

    private void SaveDroppedItems()
    {
        PlayerPrefs.DeleteAll();
        DroppedItem.SaveAllDroppedItems();
    }

    private void LoadDroppedItems()
    {
        int i = 0;
        while (PlayerPrefs.HasKey($"DroppedItem_{i}"))
        {
            string[] data = PlayerPrefs.GetString($"DroppedItem_{i}").Split('_');
            Vector2 position = new Vector2(float.Parse(data[0]), float.Parse(data[1]));
            int itemID = int.Parse(data[2]);

            Item item = ItemDatabaseManager.instance.itemDatabase.GetItemByID(itemID);
            if (item != null)
            {
                GameObject droppedItemPrefab = Resources.Load<GameObject>("DroppedItemPrefab");
                GameObject droppedItemGO = Instantiate(droppedItemPrefab, position, Quaternion.identity);
                DroppedItem droppedItem = droppedItemGO.GetComponent<DroppedItem>();
                droppedItem.SetDrop(item);
            }
            i++;
        }
    }

    private void RemoveMarkedPlants()
    {
        foreach (Plant plant in plants)
        {
            if (plant.ShouldBeRemoved())
            {
                PlantManager.instance.RemovePlant(plant);
                plants.Remove(plant);
                Destroy(plant.gameObject);
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
