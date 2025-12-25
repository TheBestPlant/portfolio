using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RiddleUIManager : MonoBehaviour
{
    public GameObject riddlePanel;
    public TMP_Text questionText;
    public Button[] answerButtons;

    private Door currentDoor;

    void Start()
    {
        riddlePanel.SetActive(false);
        /*foreach (Button btn in answerButtons)
            btn.onClick.AddListener(() => OnAnswerClick(btn));*/
    }

    public void ShowRiddle(Door door)
    {
        currentDoor = door;
        riddlePanel.SetActive(true);
        questionText.text = door.riddleQuestion;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = door.answerChoices[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswer(index));
        }
    }

    private void OnAnswer(int index)
    {
        if (index == currentDoor.correctAnswerIndex)
        {
            Destroy(currentDoor.gameObject);
        }
        else
        {
            currentDoor.failTeleport.ExecuteAction(currentDoor.gameObject);
        }

        riddlePanel.SetActive(false);
    }
}
