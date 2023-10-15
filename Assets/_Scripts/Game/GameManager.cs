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
            case GameState.StartLevel:
                HandleStartLevel();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        Debug.Log($"New Game State: {newState}");
    }

    private void HandleStartLevel()
    {
        ViewManager.Instance.Show<IngameScreen>(false);
        SpawnTilesController.Instance.SpawnTiles();
    }
}

[Serializable]
public enum GameState
{
    StartLevel = 0
}
