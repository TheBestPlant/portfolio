using UnityEngine;

[AddComponentMenu("Playground/Actions/Jumpscare")]
public class JumpscareAction : Action
{
    [Header("UI Setup")]
    public GameObject jumpscareImagePrefab; 
    public Canvas uiCanvas;        

    [Header("Sound")]
    public AudioSource audioSource;    
    public AudioClip jumpscareSound; 
    public float volume = 1f;

    [Header("Timing")]
    public float displayTime = 1f;

    public override bool ExecuteAction(GameObject dataObject)
    {
        // Show jumpscare image
        if (jumpscareImagePrefab != null && uiCanvas != null)
        {
            GameObject imageInstance = GameObject.Instantiate(jumpscareImagePrefab, uiCanvas.transform);
            GameObject.Destroy(imageInstance, displayTime);
        }
        else
        {
            Debug.LogWarning("JumpscareAction: Missing prefab or canvas.");
        }

        // Play sound using assigned AudioSource
        if (audioSource != null && jumpscareSound != null)
        {
            audioSource.clip = jumpscareSound;
            audioSource.volume = volume;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("JumpscareAction: Missing AudioSource or AudioClip.");
        }

        return true;
    }
}
