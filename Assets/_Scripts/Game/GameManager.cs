using System;
using UnityEngine;

public class GameManager : StaticInstance<GameManager>
{
    public GameState State { get; private set; }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.StartGame:
                HandleStartLevel();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        Debug.Log($"Game State: {newState}");
    }

    private void HandleStartLevel()
    {
        LevelManager.Instance.ChangeState(LevelState.SpawnTiles);
    }
}

[Serializable]
public enum GameState
{
    StartGame = 0
}
