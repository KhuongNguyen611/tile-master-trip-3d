using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class LevelManager : StaticInstance<LevelManager>
{
    [SerializeField]
    private GameObject _flowerTilePrefab;

    [SerializeField]
    private GameObject _tilesParent;

    private List<TileInfo> _tileInfoList = new();

    public LevelState State { get; private set; }

    public void StartLevel()
    {
        ViewManager.Instance.Show<IngameScreen>(false);

        TilesStackController.Instance.ShowStack();

        ChangeState(LevelState.SpawnTiles);
    }

    public void ChangeState(LevelState newState)
    {
        State = newState;
        switch (newState)
        {
            case LevelState.SpawnTiles:
                HandleSpawnTiles();
                break;
            case LevelState.DropTiles:
                HandleDropTiles();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        Debug.Log($"New Game State: {newState}");
    }

    private void HandleSpawnTiles()
    {
        ScriptableLevel scriptableLevel = ResourceSystem.Instance.GetCurrentLevel();
        List<FlowerAmount> flowerAmountList = scriptableLevel.flowerAmountList;

        flowerAmountList.ForEach(
            (flowerAmount) =>
            {
                int numberOfTriple = flowerAmount.numberOfTriples;
                ScriptableFlower scriptableFlower = flowerAmount.scriptableFlower;
                for (int i = 0; i < numberOfTriple; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        SpawnTile(scriptableFlower);
                    }
                }
            }
        );

        ChangeState(LevelState.DropTiles);
    }

    private void SpawnTile(ScriptableFlower scriptableFlower)
    {
        Vector3 spawnPoint = new Vector3(
            Random.Range(-5f, 5f),
            Random.Range(1f, 5f),
            Random.Range(-3f, 8f)
        );

        if (!Physics.CheckSphere(spawnPoint, 1f))
        {
            GameObject flowerTile = Instantiate(_flowerTilePrefab, spawnPoint, Random.rotation);
            flowerTile.transform.parent = _tilesParent.transform;
            flowerTile.transform.rotation = Random.rotation;
            TileInfo tileInfo = flowerTile.GetComponent<TileInfo>();
            tileInfo.UpdateScriptableFlower(scriptableFlower);
            _tileInfoList.Add(tileInfo);
        }
        else
        {
            SpawnTile(scriptableFlower);
        }
    }

    private void HandleDropTiles()
    {
        _tileInfoList.ForEach(
            (tileInfo) =>
            {
                tileInfo.Drop();
            }
        );
    }
}

public enum LevelState
{
    SpawnTiles = 0,
    DropTiles = 1
}
