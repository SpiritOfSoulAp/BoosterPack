using UnityEngine;
using UnityEngine.UI;

public class CardCollectionUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject cardSlotPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private CardDatabase database;
    [SerializeField] private CardFrameLibrary frameLibrary;
    [SerializeField] private CardCollection collection;
    [SerializeField] private Button backButton;
    [SerializeField] private Button cardPackButton;

    private void OnEnable()
    {
        CardCollection.OnCollectionChanged += RenderCollection;
        backButton.onClick.AddListener(() => OpenLobby());
        cardPackButton.onClick.AddListener(() => OpenSelectPackScene());
    }

    private void OnDisable()
    {
        CardCollection.OnCollectionChanged -= RenderCollection;
        backButton.onClick.RemoveListener(() => OpenLobby());
        cardPackButton.onClick.RemoveListener(() => OpenSelectPackScene());
    }

    private void Start()
    {
        RenderCollection();
    }

    public void RenderCollection()
    {
        if (cardSlotPrefab == null || gridParent == null || database == null || frameLibrary == null || collection == null)
        {
            Debug.LogError("CardCollectionUI: One or more required references are not assigned.");
            return;
        }

        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        var allCards = collection.GetAllCards();

        foreach (var kvp in allCards)
        {
            CardData card = kvp.Key;
            int count = kvp.Value;

            if (card == null) continue;

            GameObject slot = Instantiate(cardSlotPrefab, gridParent);
            CardUI ui = slot.GetComponent<CardUI>();
            if (ui != null)
            {
                Sprite frame = frameLibrary.GetFrame(card.rarity);
                ui.Setup(card, frame, count);
            }
        }
    }

    private void OpenLobby()
    {
        SceneLoader.Instance.LoadScene(
                SceneLoader.Instance.GetSceneNameFromState(GameState.Lobby),
                GameState.Lobby
            );
    }

    private void OpenSelectPackScene()
    {
        SceneLoader.Instance.LoadScene(
                SceneLoader.Instance.GetSceneNameFromState(GameState.SelectPack),
                GameState.SelectPack
            );
    }
}