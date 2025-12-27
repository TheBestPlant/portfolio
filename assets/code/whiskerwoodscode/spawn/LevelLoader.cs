using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string levelName;

    public void LoadLevel()
    {
        if (!string.IsNullOrEmpty(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogWarning("Level name is empty. Please assign a level name in the inspector.");
        }
    }
}
