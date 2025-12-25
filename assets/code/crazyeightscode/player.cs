using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerHand playerHand;
    public Deck deck;
    public Pile pile;

    public void DrawCardFromDeck()
    {
        GameObject drawnCard = deck.DrawCard();
        if (drawnCard != null)
        {
            playerHand.AddCard(drawnCard);
        }
    }

    public void PlayCard(GameObject card)
    {
        playerHand.RemoveCard(card);
        pile.AddToPile(card);
    }
}
