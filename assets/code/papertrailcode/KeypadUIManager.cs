using UnityEngine;
using TMPro;

public class KeypadUIManager : MonoBehaviour
{
    public GameObject keypadPanel;
    public TMP_Text codeDisplay;
    public GameObject keypadDisplay;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip digitSound;
    public AudioClip clearSound;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip doorDestroySound;

    private string currentInput = "";
    private string correctCode = "";
    private string targetDoorID = "";
    private bool isOpen = false;

    void Start()
    {
        if (codeDisplay != null)
        {
            codeDisplay.gameObject.SetActive(false);
            keypadPanel.gameObject.SetActive(false);
            keypadDisplay.gameObject.SetActive(false);
        }
    }

        void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Open(string code, string doorID)
    {
        currentInput = "";
        correctCode = code;
        targetDoorID = doorID;
        isOpen = true;

        keypadPanel.SetActive(true);

        if (codeDisplay != null)
        {
            codeDisplay.gameObject.SetActive(true);
            keypadDisplay.gameObject.SetActive(true);
        }

        UpdateDisplay();
    }

    public void Close()
    {
        keypadPanel.SetActive(false);
        isOpen = false;
        currentInput = "";

        if (codeDisplay != null)
        {
            codeDisplay.gameObject.SetActive(false);
            keypadDisplay.gameObject.SetActive(false);
        }
    }

    public void PressKey(string digit)
    {
        if (!isOpen || currentInput.Length >= 10) return;

        currentInput += digit;
        UpdateDisplay();

        if (audioSource != null && digitSound != null)
        {
            audioSource.PlayOneShot(digitSound);
        }
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();

        if (audioSource != null && clearSound != null)
        {
            audioSource.PlayOneShot(clearSound);
        }
    }

    public void Submit()
    {
        bool correct = currentInput == correctCode;

        if (correct)
        {
            Debug.Log("Correct code entered for door: " + targetDoorID);

            Door[] doors = FindObjectsOfType<Door>();
            foreach (Door door in doors)
            {
                if (door.doorID == targetDoorID)
                {
                    Destroy(door.gameObject);
                    if (audioSource != null && doorDestroySound != null)
                    {
                        audioSource.PlayOneShot(doorDestroySound);
                    }
                    break;
                }
            }

            if (audioSource != null && correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
            }

            Close();
        }
        else
        {
            Debug.Log("Incorrect code.");

            if (audioSource != null && incorrectSound != null)
            {
                audioSource.PlayOneShot(incorrectSound);
            }

            ClearInput();
        }
    }

    void UpdateDisplay()
    {
        codeDisplay.text = currentInput;
    }

    public void TestClick()
    {
        Debug.Log("Button clicked!");
    }
}

