using UnityEngine;
using UnityEngine.InputSystem;

public class KeypadInteractor : MonoBehaviour
{
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float interactDistance = 2f;

    private Keypad currentKeypad;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayPoint.position, transform.right, interactDistance);

        if (hit.collider != null)
        {
            Keypad keypad = hit.collider.GetComponent<Keypad>();
            if (keypad != null)
            {
                currentKeypad = keypad;

                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    currentKeypad.ShowKeypadUI();
                }
            }
            else
            {
                currentKeypad = null;
            }
        }
        else
        {
            currentKeypad = null;
        }
    }
}
