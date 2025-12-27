using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManagerShop : MonoBehaviour
{
    private void Start()
    {
        //Spawn in the right place
        if (SpawnManager.OriginatingScene == "Shop Window")
        {
            SpawnManager.NextScenePosition = new Vector3(-4.55f, -1.99f, 0f);
        }
        else if (SpawnManager.OriginatingScene == "Character Select" || SpawnManager.OriginatingScene == "Outside"||SpawnManager.OriginatingScene == "Forest" || SpawnManager.OriginatingScene == "Greenhouse")
        {
            SpawnManager.NextScenePosition = new Vector3(7.72f, -1.99f, 0f);
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
