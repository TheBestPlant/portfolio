using UnityEngine;
using TMPro;

public class NoteUIManager : MonoBehaviour
{
    public GameObject notePanel;
    public TMP_Text noteText;

    private void Start()
    {
        notePanel.SetActive(false);
    }

    public void ShowNote(string message)
    {
        notePanel.SetActive(true);
        noteText.text = message;
    }

    public void HideNote()
    {
        notePanel.SetActive(false);
    }
}
