using UnityEngine;

public class GameData : MonoBehaviour
{
    //Stores the data for quests, but for some reason hasMoved would not work
    public static GameData instance;

    public int[] pickedUp = new int[24];
    public int[] used = new int[24];
    public bool hasMoved;
    public bool watered;
    public int day;

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
    }

    void Start()
    {
        for (int i = 0; i < 24; i++)
        {
            pickedUp[i] = 0;
            used[i] = 0;
        }
        day = 1;
    }

    public void pickUpItem(int i)
    {
        pickedUp[i]++;
    }

    public void usedItem(int i)
    {
        used[i]++;
    }
    
    public void wateredAPlant()
    {
    		watered = true;
    }

    public void movedAround()
    {
    		hasMoved = true;
    }

    public void newDay()
    {
        day++;
    }
}
