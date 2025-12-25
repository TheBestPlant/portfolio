using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [Header("Balloon Prefab")]
    public GameObject balloonPrefab;

    public BalloonScript CreateBalloon(string dialogueString, bool usingButton, KeyCode button, float timeToDisappear, Color backgroundC, Color textC, Transform targetObj)
    {
        if (balloonPrefab == null)
        {
            Debug.LogError("DialogueSystem: balloonPrefab is not assigned!");
            return null;
        }

        GameObject balloonGO = Instantiate(balloonPrefab, transform);
        BalloonScript balloon = balloonGO.GetComponent<BalloonScript>();

        if (balloon == null)
        {
            Debug.LogError("DialogueSystem: balloonPrefab is missing a BalloonScript!");
            Destroy(balloonGO);
            return null;
        }

        balloon.Setup(dialogueString, usingButton, button, timeToDisappear, backgroundC, textC, targetObj);
        return balloon;
    }
}
