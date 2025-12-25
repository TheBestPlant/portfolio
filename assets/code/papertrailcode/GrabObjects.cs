using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class GrabObjects : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance = 2f;

    private GameObject grabbedObject;
    private int layerIndex;

    public NoteUIManager noteUIManager;
    public WaterThrower waterThrower;

    public AudioSource audioSource;
    public AudioClip keycardPickupSound;
    public AudioClip lighterPickupSound;
    public AudioClip notePickupSound;
    public AudioClip pickupSound;
    public AudioClip doorDestroySound;
    public AudioClip dropSound;
    public AudioClip noteDropSound;

    [Header("Movement Sound")]
    public AudioSource walkAudioSource;
    public AudioClip walkClip;

    private string acquiredKeyID = null;
    private bool justPickedUp = false;

    private Vector2 lastDirection = Vector2.right;
    private Vector3 originalGrabPointLocalPos;
    private Vector3 originalRayPointLocalPos;

    private void Start()
    {
        layerIndex = LayerMask.NameToLayer("Objects");

        if (grabPoint.localPosition == Vector3.zero)
            grabPoint.localPosition = new Vector3(0.5f, 0f, 0f);

        if (rayPoint.localPosition == Vector3.zero)
            rayPoint.localPosition = new Vector3(0.5f, 0f, 0f);

        originalGrabPointLocalPos = grabPoint.localPosition;
        originalRayPointLocalPos = rayPoint.localPosition;
    }

    void Update()
    {
        justPickedUp = false;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(moveX, moveY);

        // Handle walking sound
        if (walkAudioSource != null && walkClip != null)
        {
            if (input.sqrMagnitude > 0.01f)
            {
                if (!walkAudioSource.isPlaying)
                {
                    walkAudioSource.clip = walkClip;
                    walkAudioSource.loop = true;
                    walkAudioSource.Play();
                }
            }
            else
            {
                if (walkAudioSource.isPlaying)
                    walkAudioSource.Stop();
            }
        }

        // Cardinal direction snapping
        if (input.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
                lastDirection = moveX > 0 ? Vector2.right : Vector2.left;
            else
                lastDirection = moveY > 0 ? Vector2.up : Vector2.down;

            if (lastDirection == Vector2.right)
            {
                grabPoint.localPosition = new Vector3(1f, 0f, 0f);
                rayPoint.localPosition = new Vector3(1f, 0f, 0f);
            }
            else if (lastDirection == Vector2.left)
            {
                grabPoint.localPosition = new Vector3(-1f, 0f, 0f);
                rayPoint.localPosition = new Vector3(-1f, 0f, 0f);
            }
            else if (lastDirection == Vector2.up)
            {
                grabPoint.localPosition = new Vector3(0f, 1f, 0f);
                rayPoint.localPosition = new Vector3(0f, 1f, 0f);
            }
            else if (lastDirection == Vector2.down)
            {
                grabPoint.localPosition = new Vector3(0f, -1f, 0f);
                rayPoint.localPosition = new Vector3(0f, -1f, 0f);
            }
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, lastDirection, rayDistance);

        if (hitInfo.collider != null)
        {
            GameObject target = hitInfo.collider.gameObject;

            if (target.layer == layerIndex && grabbedObject == null)
            {
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    Keycard keycard = target.GetComponent<Keycard>();
                    if (keycard != null)
                    {
                        acquiredKeyID = keycard.doorID;
                        Destroy(target);
                        Debug.Log($"Picked up keycard for door ID: {acquiredKeyID}");
                        if (audioSource && keycardPickupSound)
                            audioSource.PlayOneShot(keycardPickupSound);
                        return;
                    }

                    if (target.CompareTag("Lighter"))
                    {
                        if (waterThrower != null)
                            waterThrower.GiveLighter();

                        Destroy(target);
                        Debug.Log("Picked up lighter.");
                        if (audioSource && lighterPickupSound)
                            audioSource.PlayOneShot(lighterPickupSound);
                        return;
                    }

                    grabbedObject = target;
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    grabbedObject.transform.position = grabPoint.position;
                    grabbedObject.transform.SetParent(transform);

                    Vector3 scale = grabbedObject.transform.localScale;
                    if (lastDirection.x != 0)
                        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(lastDirection.x);
                    grabbedObject.transform.localScale = scale;

                    Note note = grabbedObject.GetComponent<Note>();
                    if (note != null)
                    {
                        noteUIManager.ShowNote(note.noteMessage);
                        if (audioSource && notePickupSound)
                            audioSource.PlayOneShot(notePickupSound);
                    }
                    else
                    {
                        if (audioSource && pickupSound)
                            audioSource.PlayOneShot(pickupSound);
                    }

                    justPickedUp = true;
                }
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Door door = target.GetComponent<Door>();
                if (door != null)
                {
                    if (door.doorID == acquiredKeyID)
                    {
                        Destroy(door.gameObject);
                        Debug.Log($"Unlocked and destroyed door: {door.doorID}");
                        if (audioSource && doorDestroySound)
                            audioSource.PlayOneShot(doorDestroySound);
                    }
                    else
                    {
                        Debug.Log("You don't have the correct keycard for this door.");
                    }
                }
            }
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && grabbedObject != null && !justPickedUp)
        {
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            grabbedObject.transform.SetParent(null);

            Vector3 dropOffset = new Vector3(0, -0.25f, 0);
            Vector3 targetPosition = grabPoint.position + dropOffset;

            StartCoroutine(DropObjectSmoothly(grabbedObject, targetPosition));

            if (grabbedObject.CompareTag("Note"))
            {
                noteUIManager.HideNote();
                if (audioSource && noteDropSound)
                    audioSource.PlayOneShot(noteDropSound);
            }
            else
            {
                if (audioSource && dropSound)
                    audioSource.PlayOneShot(dropSound);
            }

            grabbedObject = null;
        }

        if (grabbedObject != null)
        {
            grabbedObject.transform.position = grabPoint.position;
        }

        Debug.DrawRay(rayPoint.position, lastDirection * rayDistance, Color.red);
    }

    private IEnumerator DropObjectSmoothly(GameObject obj, Vector3 dropToPosition)
    {
        float elapsed = 0f;
        float duration = 0.2f;
        Vector3 start = obj.transform.position;

        while (elapsed < duration)
        {
            if (obj == null) yield break;

            obj.transform.position = Vector3.Lerp(start, dropToPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = dropToPosition;
    }
}
