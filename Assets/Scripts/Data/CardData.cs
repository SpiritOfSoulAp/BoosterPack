using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class CardData
{
    public string cardName;
    public Rarity rarity;
    public Sprite cardImage;
    public string description;
}
