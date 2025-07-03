using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField] private Button collectionButton;
    [SerializeField] private Button cardPackButton;

    private void Start()
    {
        collectionButton.onClick.AddListener(() => OpenCollisionScene());
        cardPackButton.onClick.AddListener(() => OpenSelectPackScene());
    }

    private void OnDisable()
    {
        collectionButton.onClick.RemoveListener(() => OpenCollisionScene());
        cardPackButton.onClick.RemoveListener(() => OpenSelectPackScene());
    }

    private void OpenCollisionScene()
    {
        SceneLoader.Instance.LoadScene(
                SceneLoader.Instance.GetSceneNameFromState(GameState.Collection), 
                GameState.Collection
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
