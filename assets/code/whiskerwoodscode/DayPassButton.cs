using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DayPassButton : MonoBehaviour
{
    public string sceneToLoad = "Shop";
    public Vector3 wakeUpPosition;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private GameObject messagePopup;

    //When pressing the day pass button in the scene "Outside", the player would not spawn back in
    //So now, when you press the button in the scene "Outside", it just doesn't work and shows a popup instead :)
    public void OnDayPassButtonClicked()
    {
        if (SceneManager.GetActiveScene().name == "Outside")
        {
            ShowMessagePopup("You cannot pass the day here!");
        }
        else
        {
            if (audioSource != null && buttonClickSound != null)
            {
                audioSource.PlayOneShot(buttonClickSound);
            }
            else
            {
                Debug.LogWarning("AudioSource or ButtonClickSound is not assigned.");
            }

            if (DayManager.instance != null)
            {
                DayManager.instance.OnDayPass();
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = wakeUpPosition;
            CharacterManager.instance.UpdateCharacter(CharacterManager.instance.selectedOption);
        }
        else
        {
            Debug.LogWarning("Player not found in the new scene.");
        }
    }

    private void ShowMessagePopup(string message)
    {
        if (messagePopup != null)
        {
            messagePopup.SetActive(true);
            StartCoroutine(HidePopupAfterDelay(5f));
        }
        else
        {
            Debug.LogWarning("Message Popup not assigned.");
        }
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (messagePopup != null)
        {
            messagePopup.SetActive(false);
        }
    }
}
