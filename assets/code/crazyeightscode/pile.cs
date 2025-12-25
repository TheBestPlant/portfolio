using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    public Transform pilePosition;
    public Transform cardGoHere;
    public Deck deck;
    private SpriteRenderer spriteRenderer;

    public void AddToPile(GameObject card)
    {
        cards.Add(card);
        card.transform.position = cardGoHere.position;
        CardInHand cardInHand = card.GetComponent<CardInHand>();
        UpdateTopCardSprite();

    }

    public void TakeTopCard()
    {
        if (cards.Count > 0)
        {
            GameObject topCard = cards[cards.Count - 1];

            for (int i = 0; i < cards.Count - 1; i++)
            {
                deck.AddCard(cards[i]);
            }

            cards.Clear();
            cards.Add(topCard);

            UpdateTopCardSprite();
        }
        else
        {
            Debug.LogWarning("Pile is empty!");
        }
    }



    public Card TopCard()
    {
        if (cards.Count == 0)
        {
            Debug.LogWarning("Pile is empty!");
            return null;
        }

        CardInHand topCardInHand = cards[cards.Count - 1].GetComponent<CardInHand>();
        if (topCardInHand != null)
        {
            return topCardInHand.card;
        }
        else
        {
            Debug.LogError("CardInHand component missing on the top card in the pile.");
            return null;
        }
    }


    void Start()
    {            
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer attached to Pile GameObject.");
        }


        if (spriteRenderer != null && cards.Count > 0)
        {
            Card topCard = TopCard();
            if (topCard != null)
            {
                spriteRenderer.sprite = topCard.face;
            }
        }
    }

    public void UpdateTopCardSprite()
    {
        if (cards.Count > 0 && spriteRenderer != null)
        {
            Card topCard = TopCard();
            if (topCard != null)
            {
                Debug.Log("Updating sprite to: " + topCard.face.name);
                spriteRenderer.sprite = topCard.face;
            }
            else
            {
                Debug.LogError("Top card is null.");
            }
        }
        else
        {
            Debug.LogWarning("No cards in pile or spriteRenderer is missing.");
        }
    }
}
