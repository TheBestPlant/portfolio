using UnityEngine;

public class Flame : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip flameSound;

    void Start()
    {
        // Destroy the projectile after 1 second
        Destroy(gameObject, 1f);
        if (audioSource != null && flameSound != null)
        {
            audioSource.PlayOneShot(flameSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
