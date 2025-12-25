using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public Deck deck;
    public Pile pile;
    public PlayerHand playerHand;
    public EnemyHand enemyHand;
    public Enemy enemy;
    public bool myTurn;

    void Start()
    {
        deck.Reshuffle();
        deck.DealCards();
        pile.AddToPile(deck.DrawCard());
        myTurn = true;
    }

    void Update()
    {
        if (playerHand.IsEmpty())
        {
            SceneManager.LoadScene("Win");
            return;
        }

        if (enemyHand.IsEmpty())
        {
            SceneManager.LoadScene("Lose");
            return;
        }

        if (myTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Card"))
                    {
                        CardInHand cardInHand = hit.collider.GetComponent<CardInHand>();

                        if (cardInHand != null && cardInHand.card != null)
                        {
                            Suits suit = cardInHand.card.suit;
                            int number = cardInHand.card.number;

                            Card topCard = pile.TopCard();
                            if (topCard != null)
                            {
                                Suits pileSuit = topCard.suit;
                                int pileNum = topCard.number;

                                if (suit == pileSuit || number == pileNum)
                                {
                                    playerHand.RemoveCard(cardInHand.gameObject);
                                    pile.AddToPile(cardInHand.gameObject);
                                    pile.UpdateTopCardSprite();
                                    myTurn = false;
                                }

                                Debug.Log($"Clicked on a card: {cardInHand.card.name}, Suit: {suit}, Number: {number}");
                            }
                            else
                            {
                                Debug.LogWarning("Cannot play the card because the pile is empty.");
                            }
                        }
                    }

                    if (hit.collider.CompareTag("Deck"))
                    {
                        playerHand.AddCard(deck.DrawCard());
                    }
                }
            }
        }
        else
        {
            enemy.TakeTurn();
            myTurn = true;
        }
    }
}
