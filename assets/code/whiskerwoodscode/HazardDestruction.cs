using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDestruction : MonoBehaviour
{
    public int playerNumber = 0;
    public int scoreDecreaseAmount = 5;
    public string hazardID;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip destructionSound;

    void Start()
    {
        // Has the hazard has been destroyed previously?
        if (PlayerPrefs.GetInt(hazardID, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    // Does the hit collider belongs to this GameObject or its children?
                    if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
                    {
                        if (UIScript.instance != null)
                        {
                            if (UIScript.instance.getScore() - scoreDecreaseAmount >= 0)
                            {
                                UIScript.instance.DecreaseScoreAndDestroySprite(playerNumber, scoreDecreaseAmount, gameObject);

                                PlayerPrefs.SetInt(hazardID, 1);
                                PlayerPrefs.Save();

                                PlayDestructionSound();
                            }
                        }
                        else
                        {
                            Debug.LogError("UIScript.instance is null.");
                        }
                        break;
                    }
                }
            }
        }
    }

    void PlayDestructionSound()
    {
        if (audioSource != null && destructionSound != null)
        {
            audioSource.PlayOneShot(destructionSound);
        }
        else
        {
            Debug.LogError("AudioSource or destructionSound is null.");
        }
    }
}
