using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;
    public InventoryItemsManager inventory;
    public int day = 0;

    [SerializeField] private GameObject defaultPlantPrefab;
    [SerializeField] private List<SeedPlantMapping> seedPlantMappings;

    [SerializeField] private AudioSource plantingAudioSource;
    [SerializeField] private AudioClip plantingSound;
    [SerializeField] private AudioSource wateringAudioSource;
    [SerializeField] private AudioClip wateringSound;

    private Dictionary<int, GameObject> seedToPlantDict;

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

        InitializeSeedToPlantDictionary();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadPlants();
        if (day != GameData.instance.day)
        {
            DayManager.instance.HarvestWateredPlants();
            DayManager.instance.OnDayPass();
        }
    }

    private void InitializeSeedToPlantDictionary()
    {
        seedToPlantDict = new Dictionary<int, GameObject>();
        foreach (var mapping in seedPlantMappings)
        {
            if (mapping.plantPrefab != null)
            {
                seedToPlantDict[mapping.seedID] = mapping.plantPrefab;
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetSelectedItem() != null && GetSelectedItem().type == ItemType.Seed)
        {
            if (PlantSeed(GetSelectedItem()))
            {
                inventory.GetSelectedItem(true);
                PlayPlantingSound();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Plant plant = hit.collider.GetComponent<Plant>();
                if (plant != null)
                {
                    plant.WaterPlant();
                    SavePlant(plant, 0, true);
                    PlayWateringSound();
                }
            }
        }
    }

    private Item GetSelectedItem()
    {
        if (InventoryItemsManager.instance == null)
        {
            Debug.LogError("InventoryItemsManager instance is null.");
            return null;
        }
        return InventoryItemsManager.instance.GetSelectedItem(false);
    }

    public bool PlantSeed(Item seedItem)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        string allowedSceneName = "Greenhouse";

        if (currentSceneName != allowedSceneName)
        {
            Debug.LogWarning("Planting is not allowed in this scene.");
            return false;
        }

        if (seedItem == null)
        {
            Debug.LogError("Seed item is not assigned.");
            return false;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        GameObject plantPrefabToUse = defaultPlantPrefab;
        if (seedToPlantDict.ContainsKey(seedItem.ID))
        {
            plantPrefabToUse = seedToPlantDict[seedItem.ID];
        }

        if (plantPrefabToUse == null)
        {
            Debug.LogError("Plant prefab is not assigned.");
            return false;
        }

        GameObject newPlantGO = Instantiate(plantPrefabToUse, mousePosition, Quaternion.identity);
        Plant newPlant = newPlantGO.GetComponent<Plant>();
        if (newPlant != null)
        {
            newPlant.Initialize(seedItem);
            SavePlant(newPlant, seedItem.ID);
            return true;
        }
        else
        {
            Debug.LogError("Plant component not found on the prefab.");
            return false;
        }
    }

    private void SavePlant(Plant plant, int seedID, bool watered = false)
    {
        Vector3 position = plant.transform.position;
        string plantKey = "Plant_" + position.x + "_" + position.y;

        if (seedID != 0)
        {
            PlayerPrefs.SetInt(plantKey + "_SeedID", seedID);
        }

        PlayerPrefs.SetFloat(plantKey + "_PosX", position.x);
        PlayerPrefs.SetFloat(plantKey + "_PosY", position.y);
        PlayerPrefs.SetInt(plantKey + "_Stage", plant.CurrentStage);
        PlayerPrefs.SetInt(plantKey + "_IsWatered", plant.IsWatered ? 1 : 0);
        PlayerPrefs.SetInt(plantKey + "_LastHarvestedDay", GameData.instance.day);

        string plantKeys = PlayerPrefs.GetString("PlantKeys", "");
        if (!plantKeys.Contains(plantKey))
        {
            plantKeys += plantKey + ",";
            PlayerPrefs.SetString("PlantKeys", plantKeys);
        }
        PlayerPrefs.Save();

        Debug.Log($"Saved plant with key {plantKey} at position {position} with stage {plant.CurrentStage}, watered: {plant.IsWatered}, last harvested day: {GameData.instance.day}.");
    }

    public void LoadPlants()
    {
        string plantKeys = PlayerPrefs.GetString("PlantKeys", "");
        Debug.Log($"Loaded plant keys: {plantKeys}");

        foreach (string key in plantKeys.Split(','))
        {
            if (string.IsNullOrEmpty(key)) continue;

            int seedID = PlayerPrefs.GetInt(key + "_SeedID");
            float posX = PlayerPrefs.GetFloat(key + "_PosX");
            float posY = PlayerPrefs.GetFloat(key + "_PosY");
            int stage = PlayerPrefs.GetInt(key + "_Stage");
            bool isWatered = PlayerPrefs.GetInt(key + "_IsWatered") == 1;
            int lastHarvestedDay = PlayerPrefs.GetInt(key + "_LastHarvestedDay", -1);

            Vector3 position = new Vector3(posX, posY, 0);

            GameObject plantPrefabToUse = defaultPlantPrefab;
            if (seedToPlantDict.ContainsKey(seedID))
            {
                plantPrefabToUse = seedToPlantDict[seedID];
            }

            if (plantPrefabToUse == null)
            {
                Debug.LogError("Plant prefab is not assigned.");
                continue;
            }

            GameObject plantGO = Instantiate(plantPrefabToUse, position, Quaternion.identity);
            Plant plant = plantGO.GetComponent<Plant>();
            if (plant != null)
            {
                plant.Initialize(stage, isWatered, lastHarvestedDay);
                Debug.Log($"Loaded plant with key {key} at position {position} with stage {stage}, watered: {isWatered}, last harvested day: {lastHarvestedDay}.");
            }
            else
            {
                Debug.LogError("Plant component not found on the prefab.");
            }
        }
    }

    private void PlayPlantingSound()
    {
        if (plantingAudioSource != null && plantingSound != null)
        {
            plantingAudioSource.PlayOneShot(plantingSound);
        }
        else
        {
            Debug.LogError("Planting sound or audio source is not assigned.");
        }
    }

    private void PlayWateringSound()
    {
        if (wateringAudioSource != null && wateringSound != null && Plant.instance.playedWaterSound == false)
        {
            wateringAudioSource.PlayOneShot(wateringSound);
        }
        else
        {
            Debug.LogError("Watering sound or audio source is not assigned.");
        }
    }

    //I do not think this function works...
    public void RemovePlant(Plant plant)
    {
        Vector3 position = plant.transform.position;
        string plantKey = "Plant_" + position.x + "_" + position.y;

        if (!PlayerPrefs.HasKey(plantKey + "_SeedID"))
        {
            Debug.LogWarning($"Key {plantKey} does not exist in PlayerPrefs.");
            return;
        }

        PlayerPrefs.DeleteKey(plantKey + "_SeedID");
        PlayerPrefs.DeleteKey(plantKey + "_PosX");
        PlayerPrefs.DeleteKey(plantKey + "_PosY");
        PlayerPrefs.DeleteKey(plantKey + "_Stage");
        PlayerPrefs.DeleteKey(plantKey + "_IsWatered");
        PlayerPrefs.DeleteKey(plantKey + "_LastHarvestedDay");

        string plantKeys = PlayerPrefs.GetString("PlantKeys", "");
        List<string> newKeysList = new List<string>();

        foreach (string key in plantKeys.Split(','))
        {
            if (!string.IsNullOrEmpty(key) && key != plantKey)
            {
                newKeysList.Add(key);
            }
        }

        plantKeys = string.Join(",", newKeysList);
        PlayerPrefs.SetString("PlantKeys", plantKeys);
        PlayerPrefs.Save();

        Debug.Log($"Removed plant with key {plantKey} from PlayerPrefs.");
        Debug.Log($"Updated PlantKeys: {plantKeys}");
    }


}
