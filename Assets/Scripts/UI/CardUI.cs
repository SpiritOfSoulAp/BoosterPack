using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Image frameImage;
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private TMP_Text descrText;
    [SerializeField] private TMP_Text countText;

    public void Setup(CardData card, Sprite frameSprite, int count = 1)
    {
        if (frameImage) frameImage.sprite = frameSprite;
        if (cardImage) cardImage.sprite = card.cardImage;
        if (nameText) nameText.text = card.cardName;
        if (rarityText) rarityText.text = card.rarity.ToString();
        if (descrText) descrText.text = card.description;
        if (countText) countText.text = count > 1 ? $"x{count}" : "";
    }
}