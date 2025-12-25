using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteractor : MonoBehaviour
{
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float interactDistance = 2f;

    [SerializeField] private KeypadUIManager keypadUI;
    [SerializeField] private RiddleUIManager riddleUI;

    void Update()
    {
        // Draw debug ray in the Scene view
        Debug.DrawRay(rayPoint.position, transform.right * interactDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(rayPoint.position, transform.right, interactDistance);

        if (hit.collider != null)
        {
            Debug.Log("Raycast hit: " + hit.collider.name);

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Door door = hit.collider.GetComponent<Door>();
                if (door != null)
                {
                    switch (door.doorType)
                    {
                        case DoorType.Keypad:
                            Keypad keypad = hit.collider.GetComponent<Keypad>();
                            if (keypad != null)
                            {
                                keypad.ShowKeypadUI();
                            }
                            break;

                        case DoorType.Riddle:
                            if (riddleUI != null)
                            {
                                riddleUI.ShowRiddle(door);
                            }
                            break;

                        case DoorType.Keycard:
                            Debug.Log("Interacted with keycard door (not implemented)");
                            break;
                    }
                }
            }
        }
    }
}
