using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class CardPackUIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject selectPackPanel;
    [SerializeField] private GameObject openPackPanel;
    [SerializeField] private GameObject tapToOpenText;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private Button backButton;
    [SerializeField] private Button boosterPackButton;
    [SerializeField] private Button collectionButton;
    [SerializeField] private Button selectPackButton;

    [Header("Card Prefab")]
    [SerializeField] private Transform cardParent;
    [SerializeField] private GameObject cardSlotPrefab;

    [Header("Animation")]
    [SerializeField] private GameObject packPanel;
    [SerializeField] private RectTransform topPack;
    [SerializeField] private float delayBetweenCards = 0.2f;

    [Header("Data")]
    [SerializeField] private BoosterPackManager packManager;
    [SerializeField] private CardCollection collection;

    private int revealedCount;
    private int totalCards = 5;
    private Vector2 topPackPos = new Vector2(0, -152f);

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleStateChanged;
        HandleStateChanged(GameManager.CurrentState);

        topPackPos = topPack.anchoredPosition;

        boosterPackButton.onClick.AddListener(() => OpenAndRevealPack());
        collectionButton.onClick.AddListener(() => OnClickCollection());
        selectPackButton.onClick.AddListener(() => OnClickSelectPack());
        backButton.onClick.AddListener(() => OnClickHome());
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleStateChanged;
        boosterPackButton.onClick.RemoveListener(() => OpenAndRevealPack());
        collectionButton.onClick.RemoveListener(() => OnClickCollection());
        selectPackButton.onClick.RemoveListener(() => OnClickSelectPack());
        backButton.onClick.RemoveListener(() => OnClickHome());
    }

    private void HandleStateChanged(GameState state)
    {
        selectPackPanel.SetActive(state == GameState.SelectPack);
        openPackPanel.SetActive(state == GameState.OpenPack || state == GameState.OpenCard);
        tapToOpenText.SetActive(state == GameState.OpenPack);
        buttonPanel.SetActive(state == GameState.OpenCard);

        if (state == GameState.SelectPack)
        {
            ResetCardPack();
        }
    }
    public void OpenAndRevealPack()
    {
        StartCoroutine(OpenPackSequence());
    }
    private IEnumerator OpenPackSequence()
    {
        yield return PlayPackTearAnimation();

        var cards = packManager.OpenPack(collection);
        yield return LaunchCards(cards);

        GameManager.Instance.SetState(GameState.OpenPack);
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
        foreach (Transform child in cardParent)
            Destroy(child.gameObject);

        revealedCount = 0;
        cardParent.gameObject.SetActive(true);

        foreach (var card in cards)
        {
            var slot = Instantiate(cardSlotPrefab, cardParent);

            var canvasGroup = slot.GetComponent<CanvasGroup>();
            if (canvasGroup)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.DOFade(1f, 1f).SetEase(Ease.OutExpo);
            }

            var handler = slot.GetComponent<CardRevealHandler>();
            if (handler != null)
            {
                handler.Init(card, this);
            }

            CardRevealUI ui = slot.GetComponent<CardRevealUI>();
            if (ui != null) ui.SetupCard(card, collection);

            yield return new WaitForSeconds(delayBetweenCards);
        }
    }

    private void ResetCardSlot()
    {
        foreach (Transform child in cardParent)
            Destroy(child.gameObject);
    }

    private void ResetCardPack()
    {
        topPack.anchoredPosition = topPackPos;
    }

    public void OnCardRevealed(CardData card)
    {
        revealedCount++;
        if (revealedCount >= totalCards)
        {
            GameManager.Instance.SetState(GameState.OpenCard);
        }
    }

    public void OnClickCollection()
    {
        ResetCardSlot();

        SceneLoader.Instance.LoadScene(
                SceneLoader.Instance.GetSceneNameFromState(GameState.Collection), 
                GameState.Collection
            ); 
    }

    public void OnClickSelectPack()
    { 
        ResetCardSlot();
        GameManager.Instance.SetState(GameState.SelectPack);
    }

    public void OnClickHome()
    { 
        ResetCardSlot();
        SceneLoader.Instance.LoadScene(
               SceneLoader.Instance.GetSceneNameFromState(GameState.Lobby),
               GameState.Lobby
           );
    }

}
