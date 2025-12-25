using UnityEngine.InputSystem;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    public string correctCode = "1234";
    public string doorID = "LabDoor";

    public KeypadUIManager keypadUIManager;

    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Keyboard.current.eKey.wasPressedThisFrame)
        {
            keypadUIManager.Open(correctCode, doorID);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    public void ShowKeypadUI()
    {
        FindObjectOfType<KeypadUIManager>().Open(correctCode, doorID);
    }
}
