using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<Sprite> customerSprites;
    public List<Item> items;
    public List<string> dialogues;
    public GameObject character;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sellSound;

    void OnEnable()
    {
        SetupCharacter();
    }

    void SetupCharacter()
    {
        if (character == null)
        {
            Debug.LogError("No character assigned.");
            return;
        }

        Sprite randomSprite = GetRandomSprite();
        Item randomItem = GetRandomItem();
        string randomDialogue = GetDialogueForItem(randomItem);

        var spriteRenderer = character.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = randomSprite;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on character.");
        }

        var customer = character.GetComponent<Customer>();
        if (customer != null)
        {
            customer.itemToBuy = randomItem;
            var dialogueAction = character.GetComponent<DialogueBalloonAction>();

            if (dialogueAction != null)
            {
                dialogueAction.textToDisplay = randomDialogue;
                dialogueAction.ExecuteAction(character);
            }
            else
            {
                Debug.LogError("DialogueBalloonAction component not found on character.");
            }
        }
        else
        {
            Debug.LogError("Customer component not found on character.");
        }
    }

    Sprite GetRandomSprite()
    {
        if (customerSprites.Count > 0)
        {
            int index = Random.Range(0, customerSprites.Count);
            return customerSprites[index];
        }
        return null;
    }

    Item GetRandomItem()
    {
        if (items.Count > 0)
        {
            int index = Random.Range(0, items.Count);
            return items[index];
        }
        return null;
    }

    string GetDialogueForItem(Item item)
    {
        if (item != null && dialogues.Count > 0)
        {
            int index = items.IndexOf(item);
            if (index >= 0 && index < dialogues.Count)
            {
                return dialogues[index];
            }
        }
        return "No dialogue available.";
    }

    //I do not think this one works
    public void PlaySellSound()
    {
        if (audioSource != null && sellSound != null)
        {
            audioSource.PlayOneShot(sellSound);
        }
    }

    public void SellItem()
    {
        PlaySellSound();
    }
}
