using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening; 

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private GameObject loadingUI;
    [SerializeField] private Slider loadingSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName, GameState? targetState = null)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            if (targetState.HasValue)
                GameManager.Instance.SetState(targetState.Value);
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneName, targetState));
    }

    private IEnumerator LoadSceneAsync(string sceneName, GameState? targetState)
    {
        CanvasGroup canvasGroup = loadingUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = loadingUI.AddComponent<CanvasGroup>();
        }
        if (loadingUI) loadingUI.SetActive(true);
        {
            
            yield return canvasGroup.DOFade(1f, 0.5f).WaitForCompletion();   
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (loadingSlider) loadingSlider.value = progress;

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        if (targetState.HasValue)
            GameManager.Instance.SetState(targetState.Value);

        if (loadingUI)
        {
            yield return canvasGroup.DOFade(0f, 0.5f).WaitForCompletion(); 
            loadingUI.gameObject.SetActive(false);
        }
    }

    public string GetSceneNameFromState(GameState state)
    {
        return state switch
        {
            GameState.Init => "InitScene",
            GameState.Lobby => "LobbyScene",
            GameState.SelectPack => "CardPackScene",
            GameState.OpenPack => "CardPackScene",
            GameState.OpenCard => "CardPackScene",
            GameState.Collection => "CollectionScene",
            _ => "LobbyScene"
        };
    }
}
