using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManagerForest : MonoBehaviour
{
    private void Start()
    {
        DroppedItem.LoadAllDroppedItems(transform);
        Debug.Log("Loading Dropped Items in Forest");
        
        //Spawn in the right place
        if (SpawnManager.OriginatingScene == "Outside")
        {
            SpawnManager.NextScenePosition = new Vector3(-19.27f, -16.16f, 0f);
        }

        //Move player to spawn
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = SpawnManager.NextScenePosition;
        }
        else
        {
            Debug.LogWarning("Player not found in the new scene.");
        }
        DroppedItem.SaveAllDroppedItems();
        Debug.Log("Saving Dropped Items in Forest");
    }
}
