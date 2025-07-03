using UnityEngine;
using System;

public enum GameState
{
    Init,
    Lobby,
    SelectPack,
    OpenPack,
    OpenCard,
    Collection
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState currentState;

    public static GameManager Instance { get; set; }
    public static event Action<GameState> OnStateChanged;

    public static GameState CurrentState { get; set; }

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

    private void Start()
    {
        SetState(GameState.Init);
        InitToLobby();
    }

    private void InitToLobby()
    {
        SceneLoader.Instance.LoadScene(
                SceneLoader.Instance.GetSceneNameFromState(GameState.Lobby),
                GameState.Lobby
            );
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }

}
