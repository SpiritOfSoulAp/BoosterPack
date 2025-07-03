using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardRevealUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image cardBackImage; 
    [SerializeField] private Image cardImage;
    [SerializeField] private Image frameImage;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button cardButton;

    [Header("Config")]
    public CardFrameLibrary frameLibrary;

    private CardData currentCard;
    private bool isRevealed = false;

    public void SetupCard(CardData card, CardCollection collection)
    {
        currentCard = card;
        ShowBack();
        cardButton.onClick.AddListener(() => RevealCard(collection));
    }
    private void OnDisable()
    {
        cardButton.onClick.RemoveAllListeners();
    }

    private void RevealCard(CardCollection collection)
    {
        if (isRevealed) return;
        Debug.Log($"Revealing card: {currentCard.cardName}");
        isRevealed = true;

        Sequence flip = DOTween.Sequence();
        flip.Append(transform.DOScaleX(0f, 0.2f));
        flip.AppendCallback(() => {
            if (!cardImage) return;

            cardImage.sprite = currentCard.cardImage;
            if (cardNameText) cardNameText.text = currentCard.cardName;
            if (rarityText) rarityText.text = currentCard.rarity.ToString();
            if (descriptionText) descriptionText.text = currentCard.description.ToString();
            if (frameLibrary && frameImage)
                frameImage.sprite = frameLibrary.GetFrame(currentCard.rarity);

            if (cardBackImage) cardBackImage.enabled = false;
            if (frameImage) frameImage.enabled = true;
        });
        flip.Append(transform.DOScaleX(1f, 0.2f));
    }

    private void ShowBack()
    {
        if (cardBackImage && frameLibrary && frameLibrary.cardBack)
        {
            cardBackImage.sprite = frameLibrary.cardBack;
            cardBackImage.enabled = true;
        }

        transform.localScale = new Vector3(1, 1, 1);
    }
}