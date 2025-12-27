using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class QuestListManager : MonoBehaviour
{
    public static QuestListManager instance;

    public GameObject questItemPrefab;
    public Transform questListParent;
    public List<Quest> quests = new List<Quest>();
    public AudioClip questCompletedSound;
    public string sceneToLoad;

    private Dictionary<int, GameObject> questGameObjects = new Dictionary<int, GameObject>();
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DisplayQuests();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void DisplayQuests()
    {
        foreach (Transform child in questListParent)
        {
            Destroy(child.gameObject);
        }

        questGameObjects.Clear();

        Quest firstIncompleteQuest = null;
        foreach (Quest quest in quests)
        {
            if (!quest.isCompleted)
            {
                firstIncompleteQuest = quest;
                break;
            }
        }

        if (firstIncompleteQuest != null)
        {
            GameObject questItem = Instantiate(questItemPrefab, questListParent);
            questItem.GetComponent<Text>().text = firstIncompleteQuest.title;
            questGameObjects[firstIncompleteQuest.item1Index] = questItem;
            questGameObjects[firstIncompleteQuest.item2Index] = questItem;
            questGameObjects[firstIncompleteQuest.item3Index] = questItem;
        }
    }

    public void AddQuest(string title, string description)
    {
        Quest newQuest = new Quest(title, description, quests.Count, 0, 0, 0, 0, 0, 0, 0, 0);
        quests.Add(newQuest);
        DisplayQuests();
    }

    public void questCompleted()
    {
        foreach (Quest quest in quests)
        {
            if (quest.isCompleted && questGameObjects.TryGetValue(quest.item1Index, out GameObject questItem))
            {
                Text questText = questItem.GetComponent<Text>();
                questText.text = "[DONE] " + quest.title;
                PlayCompletionSound();
                DisplayQuests();
            }
        }

        //if all the quests are completed, we load the end scene.
        if (AreAllQuestsCompleted())
        {
            LoadNextScene();
        }
    }

    void PlayCompletionSound()
    {
        if (audioSource != null && questCompletedSound != null)
        {
            audioSource.PlayOneShot(questCompletedSound);
        }
    }

    bool AreAllQuestsCompleted()
    {
        foreach (Quest quest in quests)
        {
            if (!quest.isCompleted)
            {
                return false;
            }
        }
        return true;
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene to load is not specified.");
        }
    }

    void Update()
    {
        if (GameData.instance == null)
        {
            Debug.LogError("GameData instance is null.");
            return;
        }

        foreach (Quest quest in quests)
        {
            if (quest == null)
            {
                Debug.LogError("Quest is null.");
                continue;
            }

            if (GameData.instance.pickedUp[quest.item1Index] >= quest.numItem1Collected && GameData.instance.used[quest.item1Index] >= quest.numItem1Used)
            {
                if (GameData.instance.pickedUp[quest.item2Index] >= quest.numItem2Collected && GameData.instance.used[quest.item2Index] >= quest.numItem2Used)
                {
                    if (GameData.instance.pickedUp[quest.item3Index] >= quest.numItem3Collected && GameData.instance.used[quest.item3Index] >= quest.numItem3Used)
                    {
                        if (!quest.isCompleted)
                        {
                            quest.isCompleted = true;
                            questCompleted();
                        }
                    }
                }
            }
        }
    }
}
