using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public float maxHandWidth = 25f;
    public float maxHandHeight = 175f;
    public Transform center;

    private List<GameObject> hand = new List<GameObject>();

    void Start()
    {
        UpdateCardPositions();
    }

    public void AddCard(GameObject card)
    {
        if (card == null)
        {
            Debug.LogError("Null Card added to Player Hand");
            return;
        }

        CardInHand cardInHandComponent = card.GetComponent<CardInHand>();
        if (cardInHandComponent != null)
        {
            SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = cardInHandComponent.card.face;
            hand.Add(card);
            UpdateCardPositions();
            Debug.Log($"Card added to hand: {card.name}");
        }
        else
        {
            Debug.LogError("CardInHand component missing on the card GameObject");
        }
    }

    public void RemoveCard(GameObject card)
    {
        hand.Remove(card);
        UpdateCardPositions();
    }

    private void UpdateCardPositions()
    {
        if (hand.Count == 0) return;

        float cardWidth = maxHandWidth / hand.Count;
        float cardHeight = maxHandHeight;

        float halfHandWidth = (hand.Count - 1) * cardWidth / 2f;

        for (int i = 0; i < hand.Count; i++)
        {
            float cardPositionX = center.position.x - halfHandWidth + i * cardWidth;
            float cardPositionY = center.position.y;

            Vector3 cardPosition = new Vector3(cardPositionX, cardPositionY, transform.position.z);
            hand[i].transform.position = cardPosition;
        }
    }

    public bool IsEmpty()
    {
        if(hand.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

