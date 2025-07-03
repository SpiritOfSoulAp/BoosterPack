using UnityEngine;

[CreateAssetMenu(fileName = "CardFrameLibrary", menuName = "BoosterPack/CardFrameLibrary")]
public class CardFrameLibrary : ScriptableObject
{
    [Header("Card Frames")]
    public Sprite rareFrame;
    public Sprite epicFrame;
    public Sprite legendaryFrame;
    public Sprite commonFrame;

    [Header("Card Back")]
    public Sprite cardBack;

    public Sprite GetFrame(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => commonFrame,
            Rarity.Rare => rareFrame,
            Rarity.Epic => epicFrame,
            Rarity.Legendary => legendaryFrame,
            _ => null
        };
    }
}