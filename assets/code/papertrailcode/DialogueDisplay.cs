using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueDisplay : Action
{
    public TextMeshProUGUI dialogueText;

    [TextArea(3, 10)]
    public string message = "Hello world";

    public float displayDuration = 5f;

    private Coroutine hideCoroutine;

    private void Start()
    {
        dialogueText.gameObject.SetActive(false);
    }

    public override bool ExecuteAction(GameObject other)
    {
        ShowDialogue(message, displayDuration);
        return true;
    }

    public void ShowDialogue(string msg, float duration)
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        dialogueText.gameObject.SetActive(true);
        dialogueText.text = msg;
        hideCoroutine = StartCoroutine(HideAfterDelay(duration));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogueText.gameObject.SetActive(false);
        hideCoroutine = null;
    }
}
