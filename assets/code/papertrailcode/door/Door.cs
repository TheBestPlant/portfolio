using UnityEngine;

public enum DoorType { Keycard, Keypad, Riddle }

public class Door : MonoBehaviour
{
    public string doorID;
    public DoorType doorType;

    // For riddles
    [TextArea] public string riddleQuestion;
    public string[] answerChoices = new string[4];
    public int correctAnswerIndex = 0;
    public TeleportAction failTeleport;
}
