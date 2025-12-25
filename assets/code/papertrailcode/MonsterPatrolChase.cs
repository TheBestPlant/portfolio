using UnityEngine;

public class MonsterPatrolChase : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Chase Settings")]
    public Transform player;
    public float chaseSpeed = 4f;
    public float detectionRange = 5f;
    public float fieldOfViewAngle = 120f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip monsterChaseSound;
    public AudioClip returnToPatrolSound;
    // public AudioClip monsterIdleSound;

    private Vector3 currentTarget;
    private Rigidbody2D rb;
    private Animator animator;

    private bool chasing = false;
    private bool wasChasing = false;

    // private float idleTimer = 0f;
    // public float idleInterval = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentTarget = pointB.position;
    }

    void Update()
    {
        if (player == null) return;

        bool canSee = CanSeePlayer();

        if (canSee && !wasChasing)
        {
            audioSource.clip = monsterChaseSound;
            audioSource.Play();
        }
        else if (!canSee && wasChasing)
        {
            audioSource.clip = returnToPatrolSound;
            audioSource.Play();
        }

        chasing = canSee;
        wasChasing = chasing;

        if (animator != null)
            animator.SetBool("IsChasing", chasing);

        if (chasing)
        {
            // idleTimer = 0f;
            ChasePlayer();
        }
        else
        {
            Patrol();

            /*
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleInterval)
            {
                audioSource.clip = monsterIdleSound;
                audioSource.Play();
                idleTimer = 0f;
            }
            */
        }
    }

    private bool CanSeePlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
            return false;

        // Check field of view angle
        Vector2 facingDirection = rb.linearVelocity.x >= 0 ? Vector2.right : Vector2.left;
        float angle = Vector2.Angle(facingDirection, directionToPlayer);

        if (angle < fieldOfViewAngle / 2f)
        {
            return true;
        }

        return false;
    }

    private void Patrol()
    {
        float step = patrolSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);

        // Flip sprite
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (currentTarget.x > transform.position.x ? 1 : -1);
        transform.localScale = scale;

        // Switch patrol point
        if (Vector2.Distance(transform.position, currentTarget) < 0.1f)
        {
            currentTarget = (currentTarget == pointB.position) ? pointA.position : pointB.position;
        }
    }

    private void ChasePlayer()
    {
        float step = chaseSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.position, step);

        // Flip sprite
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (player.position.x > transform.position.x ? 1 : -1);
        transform.localScale = scale;
    }
}
