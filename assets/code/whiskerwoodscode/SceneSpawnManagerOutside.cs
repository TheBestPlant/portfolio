using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManagerOutside : MonoBehaviour
{
    private void Start()
    {
        //Spawn in the right place
        if (SpawnManager.OriginatingScene == "Shop")
        {
            SpawnManager.NextScenePosition = new Vector3(-13.79f, -13.05f, 0f);
        }
        else if (SpawnManager.OriginatingScene == "Greenhouse")
        {
            SpawnManager.NextScenePosition = new Vector3(12.46f, -13.05f, 0f);
        }
        else if (SpawnManager.OriginatingScene == "Forest")
        {
            SpawnManager.NextScenePosition = new Vector3(-13.14f, 11.18f, 0f);
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
    }
}
