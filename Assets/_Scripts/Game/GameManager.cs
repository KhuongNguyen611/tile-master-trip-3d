using System;
using UnityEngine;

public class GameManager : StaticInstance<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    [SerializeField]
    private TutorialControl _tutorialControl;

    // Kick the game off with the first state
    void Start() => ChangeState(GameState.GetPlayerProgress);

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.GetPlayerProgress:
                HandleGetPlayer();
                break;
            case GameState.Tutorial:
                HandleTutorial();
                break;
            case GameState.PrepareLevel:
                HandlePrepareLevel();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New state: {newState}");
    }

    private void HandleGetPlayer()
    {
        ScriptablePlayerProgress playerProgress = ResourceSystem.Instance.PlayerProgress;
        bool IsCompleteTutorial = !playerProgress.currentLevel.Equals(0);
        if (IsCompleteTutorial)
        {
            ViewManager.Instance.Show<HomeScreen>();
        }
        else
        {
            ChangeState(GameState.Tutorial);
        }
    }

    private void HandleTutorial()
    {
        _tutorialControl.gameObject.SetActive(true);

        ChangeState(GameState.PrepareLevel);
    }

    private void HandlePrepareLevel()
    {
        ViewManager.Instance.Show<IngameScreen>();
    }
}

[Serializable]
public enum GameState
{
    GetPlayerProgress = 0,
    Tutorial = 1,
    PrepareLevel,
}
