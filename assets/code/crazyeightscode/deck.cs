using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    public Transform deckPosition;
    public GameObject pile;
    public GameObject player;
    public GameObject enemy;
    public GameObject enemyHand;

    private System.Random rng = new System.Random();

    public void Reshuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = rng.Next(i, cards.Count);
            GameObject temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public GameObject DrawCard()
    {
        Pile pileComponent = pile.GetComponent<Pile>();
        if (cards.Count <= 0)
        {
            pileComponent.TakeTopCard();
            Reshuffle();
        }

        GameObject drawnCard = cards[cards.Count - 1];
        cards.RemoveAt(cards.Count - 1);
        return drawnCard;
    }

    public void DealCards()
    {
        Player playerComponent = player.GetComponent<Player>();
        Pile pileComponent = pile != null ? pile.GetComponent<Pile>() : null;
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        EnemyHand enemyHandComponent = enemyHand.GetComponent<EnemyHand>();

        if (pileComponent == null)
        {
            Debug.LogError("Pile component is missing.");
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            if (cards.Count > 0)
            {
                GameObject cardToAdd = DrawCard();
                if (cardToAdd != null)
                {
                    CardInHand cardInHandComponent = cardToAdd.GetComponent<CardInHand>();
                    if (cardInHandComponent != null)
                    {
                        playerComponent.playerHand.AddCard(cardToAdd);
                        cardToAdd.transform.position = playerComponent.playerHand.transform.position;
                    }
                    else
                    {
                        Debug.LogError("CardInHand component missing on card.");
                    }
                }
            }
            else
            {
                Debug.LogWarning("Not enough cards in the deck to deal!");
                break;
            }

            if (cards.Count > 0)
            {
                GameObject cardToAdd = DrawCard();
                if (cardToAdd != null)
                {
                    if (enemyHandComponent != null)
                    {
                        enemyHandComponent.AddCard(cardToAdd);
                        //enemyHandComponent.UpdateSprite(cardToAdd);
                        cardToAdd.transform.position = enemyHandComponent.center.position;
                    }
                    else
                    {
                        Debug.LogError("EnemyHand component is missing.");
                    }
                }
            }
        }
        enemyHandComponent.UpdateCardPositions();
    }

    public void AddCard(GameObject card)
    {
        cards.Add(card);
        CardInHand cardInHand = card.GetComponent<CardInHand>();
    }
}
