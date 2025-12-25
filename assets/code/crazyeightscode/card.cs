using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card", order = 1)]
public class Card : ScriptableObject
{
    public Suits suit;
    public int number;
    public Sprite face;
    public Sprite back;
}

public enum Suits
{
    Heart,
    Diamond,
    Spade,
    Club
}
