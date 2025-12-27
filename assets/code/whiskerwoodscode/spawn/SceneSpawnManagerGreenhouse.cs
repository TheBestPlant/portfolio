using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManagerGreenhouse : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Loading Plants Spawn Manager");
        PlantManager.instance.LoadPlants();

        //Spawn in the right place
        if (SpawnManager.OriginatingScene == "Outside")
        {
            SpawnManager.NextScenePosition = new Vector3(0f, -2.65f, 0f);
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
