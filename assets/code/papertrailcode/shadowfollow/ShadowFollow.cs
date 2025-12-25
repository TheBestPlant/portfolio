using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
    //Did not get this script to work in time
    public Transform anchorPoint;
    public Transform spriteToMove;
    public Transform player;
    public BoxCollider2D roomBounds;

    [Range(0f, 1f)]
    public float blockFraction = 0.25f;

    private Vector3 initialLocalPosition;

    void Start()
    {
        if (spriteToMove == null) spriteToMove = transform;
        initialLocalPosition = spriteToMove.localPosition;
    }

    void Update()
    {
        if (!roomBounds.bounds.Contains(player.position))
        {
            // Player is not in the room — reset sprite position
            spriteToMove.localPosition = initialLocalPosition;
            return;
        }

        Vector2 direction = GetDirectionFromCenterToPlayer();

        Vector3 offset = new Vector3(direction.x, direction.y, 0f) * blockFraction;
        spriteToMove.localPosition = initialLocalPosition + offset;
    }

    Vector2 GetDirectionFromCenterToPlayer()
    {
        Vector2 toPlayer = player.position - anchorPoint.position;
        Vector2 direction = Vector2.zero;

        // Determine which axis is stronger (dominant movement direction)
        if (Mathf.Abs(toPlayer.x) > Mathf.Abs(toPlayer.y))
        {
            direction.x = toPlayer.x > 0 ? 1 : -1;
        }
        else
        {
            direction.y = toPlayer.y > 0 ? 1 : -1;
        }

        return direction;
    }
}
