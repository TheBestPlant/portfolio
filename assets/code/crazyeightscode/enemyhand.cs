using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
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
            Debug.LogError("Null Card added to Enemy Hand");
            return;
        }

        hand.Add(card);
        UpdateCardPositions();

        /*CardInHand cardInHandComponent = card.GetComponent<CardInHand>();
        if (cardInHandComponent != null)
        {
            SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = cardInHandComponent.card.back;
                UpdateSprite(card);
            }
            else
            {
                Debug.LogError("SpriteRenderer missing on the card GameObject.");
            }

            Debug.Log($"Card added to Enemy Hand: {card.name}");
        }
        else
        {
            Debug.LogError("CardInHand component missing on the card GameObject.");
        }*/
    }

    /*public void UpdateSprite(GameObject card)
    {
        CardInHand cardInHandComponent = card.GetComponent<CardInHand>();
        if (cardInHandComponent != null)
        {
            SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Debug.Log($"Before Update: {spriteRenderer.sprite.name}");
                Debug.Log($"Setting sprite to back: {cardInHandComponent.card.back.name}");

                spriteRenderer.sprite = cardInHandComponent.card.back;
                spriteRenderer.enabled = false;
                spriteRenderer.enabled = true;
                card.SetActive(false);
                card.SetActive(true);

                Debug.Log($"After Update: {spriteRenderer.sprite.name}");
            }
            else
            {
                Debug.LogError("SpriteRenderer missing on the card GameObject.");
            }
        }
        else
        {
            Debug.LogError("CardInHand component missing on the card GameObject.");
        }
    }*/

    public void RemoveCard(GameObject card)
    {
        hand.Remove(card);
        UpdateCardPositions();
    }

    public void UpdateCardPositions()
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

    public List<GameObject> GetHand()
    {
        return hand;
    }

    public bool IsEmpty()
    {
        if (hand.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
