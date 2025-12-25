using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveDirection;

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
        FlipSprite(direction);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void FlipSprite(Vector2 direction)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        sr.flipX = false;
        sr.flipY = false;

        if (direction.x < 0)
        {
            // Flip horizontally when facing left
            sr.flipX = true;
        }
        else if (direction.y > 0)
        {
            // Facing up, no flip
            sr.flipY = false;
        }
        else if (direction.y < 0)
        {
            // Facing down, no flip - looks weird
            sr.flipX = false;
            sr.flipY = false;
        }
    }
}
