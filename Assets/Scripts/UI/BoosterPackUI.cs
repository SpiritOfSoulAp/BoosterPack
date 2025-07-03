using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoosterPackUI : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private BoosterPackManager packManager;
    [SerializeField] private CardCollection collection;
    [SerializeField] private float delayBetweenCards = 0.2f;

    [Header("UI References")]
    [SerializeField] private Transform cardParent;
    [SerializeField] private GameObject cardSlotPrefab;
    [SerializeField] private GameObject packPanel;
    [SerializeField] private RectTransform topPack;

    public void OpenAndRevealPack()
    {
        StartCoroutine(OpenPackSequence());
    }

    private IEnumerator OpenPackSequence()
    {
        yield return PlayPackTearAnimation();

        List<CardData> cards = packManager.OpenPack(collection);
        yield return LaunchCards(cards);
    }

    private IEnumerator PlayPackTearAnimation()
    {
        if (packPanel && topPack)
        {
            topPack.DOAnchorPosY(-108, 0.8f).SetEase(Ease.OutExpo); 
            yield return new WaitForSeconds(1.5f);
            packPanel.SetActive(false);
        }
    }

    private IEnumerator LaunchCards(List<CardData> cards)
    {
        cardParent.gameObject.SetActive(true);

        foreach (var card in cards)
        {
            GameObject slot = Instantiate(cardSlotPrefab, cardParent);
            slot.GetComponent<CanvasGroup>().alpha = 0f;
            slot.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetEase(Ease.OutExpo);

            CardRevealUI ui = slot.GetComponent<CardRevealUI>();
            if (ui != null) ui.SetupCard(card, collection);

            yield return new WaitForSeconds(delayBetweenCards);
        }
    }
}