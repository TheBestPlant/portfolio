using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyHand enemyHand;
    public Deck deck;
    public Pile pile;

    public void DrawCardFromDeck()
    {
        GameObject drawnCard = deck.DrawCard();
        if (drawnCard != null)
        {
            enemyHand.AddCard(drawnCard);
        }
    }

    public void PlayCard(GameObject card)
    {
        enemyHand.RemoveCard(card);
        pile.AddToPile(card);
    }

    public void TakeTurn()
    {

        Card topCard = pile.TopCard();

        if (topCard == null)
        {
            Debug.LogWarning("The pile is empty. Enemy cannot take its turn.");
            return;
        }

        GameObject matchingCard = null;

        foreach (GameObject cardObject in enemyHand.GetHand())
        {
            CardInHand cardInHand = cardObject.GetComponent<CardInHand>();
            if (cardInHand != null)
            {
                Card card = cardInHand.card;
                if (card != null && (card.suit == topCard.suit || card.number == topCard.number))
                {
                    matchingCard = cardObject;
                    break;
                }
            }
        }

        if (matchingCard != null)
        {
            PlayCard(matchingCard);
            Debug.Log($"Enemy played {matchingCard.GetComponent<CardInHand>().card.name}");
            return;
        }

        while (deck.cards.Count > 0)
        {
            GameObject drawnCard = deck.DrawCard();
            enemyHand.AddCard(drawnCard);

            CardInHand cardInHand = drawnCard.GetComponent<CardInHand>();
            if (cardInHand != null)
            {
                Card card = cardInHand.card;
                if (card != null && (card.suit == topCard.suit || card.number == topCard.number))
                {
                    PlayCard(drawnCard);
                    Debug.Log($"Enemy drew and played {card.name}");
                    return;
                }
            }
        }

        Debug.Log("Enemy has no valid cards and cannot play.");
    }
}
