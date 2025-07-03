using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : MonoBehaviour
{    
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject cardSlotPrefab;
    [SerializeField] private Button backButton;
    [SerializeField] private Button cardPackButton;

    private void Start()
    {
        backButton.onClick.AddListener(() => OpenLobby());
        cardPackButton.onClick.AddListener(() => OpenSelectPackScene());
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(() => OpenLobby());
        cardPackButton.onClick.RemoveListener(() => OpenSelectPackScene());
    }

    private void OpenLobby()
    {
        SceneLoader.Instance.LoadScene(
                SceneLoader.Instance.GetSceneNameFromState(GameState.SelectPack),
                GameState.SelectPack
            );
    }

    private void OpenSelectPackScene()
    {
        SceneLoader.Instance.LoadScene(
                SceneLoader.Instance.GetSceneNameFromState(GameState.SelectPack),
                GameState.SelectPack
            );
    }

    public void DisplayCollection(CardCollection collection)
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in collection.GetAllCards())
        {
            GameObject slot = Instantiate(cardSlotPrefab, gridParent);
            CardRevealUI ui = slot.GetComponent<CardRevealUI>();
            if (ui != null) ui.SetupCard(entry.Key, collection);
            Text countText = slot.GetComponentInChildren<Text>();
            countText.text = "x" + entry.Value;
        }
    }
}
