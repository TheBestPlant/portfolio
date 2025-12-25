using UnityEngine;

public class WaterThrower : MonoBehaviour
{
    public GameObject waterPrefab;
    public Transform throwPoint;

    public int maxWater = 5;
    private int currentWater;

    private Vector2 facingDirection = Vector2.down;
    private float lastThrowTime;
    public float throwCooldown = 0.5f;

    public GameObject flamePrefab;
    public Vector2 flameOffset = new Vector2(1f, 0); 

    public AudioSource audioSource;
    public AudioClip waterThrowSound;
    public AudioClip waterRefillSound;
    public AudioClip fireSound;

    private bool hasLighter = false;
    private GameObject activeFlame;

    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance = 2f;

    private bool canRefill = false;

    void Start()
    {
        currentWater = maxWater;
    }

    void Update()
    {
        UpdateFacingDirection();

        // Refill station detection
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, facingDirection, rayDistance);
        if (hitInfo.collider != null && hitInfo.collider.CompareTag("RefillStation"))
        {
            canRefill = true;
            Debug.DrawRay(rayPoint.position, facingDirection * rayDistance, Color.green);
        }
        else
        {
            canRefill = false;
            Debug.DrawRay(rayPoint.position, facingDirection * rayDistance, Color.red);
        }

        // Shift key to throw water or place flame
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastThrowTime + throwCooldown)
        {
            if (hasLighter)
            {
                ToggleFlame();
                if (audioSource != null && fireSound != null)
                {
                    audioSource.PlayOneShot(fireSound);
                }
                return;
            }
            else
            {
                TryThrowWater();
            }
        }

        // Refill
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (canRefill)
            {
                RefillWater();
                if (audioSource != null && waterRefillSound != null)
                {
                    audioSource.PlayOneShot(waterRefillSound);
                }
                return;
            }
            else
            {
                Debug.Log("Not near a refill station.");
            }
        }
    }

    void UpdateFacingDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0) facingDirection = new Vector2(x, 0);
        else if (y != 0) facingDirection = new Vector2(0, y);
    }

    void TryThrowWater()
    {
        if (currentWater > 0)
        {
            GameObject water = Instantiate(waterPrefab, throwPoint.position, Quaternion.identity);
            water.GetComponent<WaterProjectile>().SetDirection(facingDirection);
            currentWater--;
            lastThrowTime = Time.time;

            // Trigger attack animation
            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Attack");
                anim.SetFloat("MoveX", facingDirection.x);
                anim.SetFloat("MoveY", facingDirection.y);
            }

            if (audioSource != null && waterThrowSound != null)
            {
                audioSource.PlayOneShot(waterThrowSound);
            }
        }
        else
        {
            Debug.Log("Out of water! Refill at a station.");
        }
    }

    public void RefillWater()
    {
        currentWater = maxWater;
        Debug.Log("Water refilled!");
    }

    void ToggleFlame()
    {
        if (activeFlame == null)
        {
            Vector3 spawnPos = transform.position + (Vector3)(facingDirection.normalized * flameOffset.magnitude);
            activeFlame = Instantiate(flamePrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Destroy(activeFlame);
            activeFlame = null;
        }

        lastThrowTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lighter"))
        {
            GiveLighter();
            Destroy(other.gameObject);
        }
    }

    public void GiveLighter()
    {
        hasLighter = true;
        Debug.Log("Picked up a lighter! You can now use fire instead of water.");
    }
}
