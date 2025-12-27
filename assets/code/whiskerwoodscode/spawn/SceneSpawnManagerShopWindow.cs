using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManagerShopWindow : MonoBehaviour
{
    private void Start()
    {
        //Spawn in the right place
        if (SpawnManager.OriginatingScene == "Shop")
        {
            SpawnManager.NextScenePosition = new Vector3(-4.55f, -1.99f, 0f);
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
