using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldCard : MonoBehaviour
{

    public Card card;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer != null && card != null)
        {
            spriteRenderer.sprite = card.face;
        }
    }

    void Update()
    {
        
    }
}
