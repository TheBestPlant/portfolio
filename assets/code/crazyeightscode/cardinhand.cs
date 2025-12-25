using UnityEngine;
using UnityEngine.EventSystems;

public class CardInHand : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform originalParent;

    public Card card;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;
        transform.SetParent(null);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPosition.z = 0;
        transform.position = worldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (originalParent != null)
        {
            transform.position = startPosition;
            transform.SetParent(originalParent);
        }
    }
}
