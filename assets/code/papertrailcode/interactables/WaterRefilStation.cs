using UnityEngine;

public class WaterRefillStation : MonoBehaviour
{
    private bool playerInside = false;
    private WaterThrower playerThrower;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (playerThrower != null)
            {
                playerThrower.RefillWater();
                Debug.Log("Water refilled!");
            }
            else
            {
                Debug.LogWarning("WaterThrower reference is missing!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerThrower = other.GetComponent<WaterThrower>();
            Debug.Log("Player entered refill station");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerThrower = null;
            Debug.Log("Player left refill station");
        }
    }
}
