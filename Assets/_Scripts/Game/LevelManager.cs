using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class LevelManager : StaticInstance<LevelManager>
{
    public LevelState State { get; private set; }

    [SerializeField]
    private GameObject _flowerTilePrefab;

    [SerializeField]
    private GameObject _tilesParent;

    private List<TileInfo> _tileInfoList = new();

    [SerializeField]
    private IngameScreen _ingameScreen;

    private ObjectPool<TileInfo> _tilesPool;

    void Start()
    {
        _tilesPool = new ObjectPool<TileInfo>(
            () =>
            {
                return Instantiate(_flowerTilePrefab, _tilesParent.transform)
                    .GetComponent<TileInfo>();
            },
            tileInfo =>
            {
                tileInfo.ChangeState(TileState.Spawn);
            },
            tileInfo =>
            {
                tileInfo.ChangeState(TileState.Match);
            },
            tileInfo =>
            {
                Destroy(tileInfo.gameObject);
            },
            false,
            50,
            100
        );
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

        Debug.Log($"Level State: {newState}");
    }

    private void HandleSpawnTiles()
    {
        ViewManager.Instance.Show<IngameScreen>(false);

        TilesStackController.Instance.ShowStack();

        _tilesParent.SetActive(true);

        StartCoroutine(SpawnTiles());
    }

    private IEnumerator<WaitForSeconds> SpawnTiles()
    {
        ScriptableLevel scriptableLevel = ResourceSystem.Instance.GetCurrentLevel();
        List<FlowerAmount> flowerAmountList = scriptableLevel.flowerAmountList;

        for (int i = 0; i < flowerAmountList.Count; i++)
        {
            FlowerAmount flowerAmount = flowerAmountList[i];
            int numberOfTriple = flowerAmount.numberOfTriples;
            ScriptableFlower scriptableFlower = flowerAmount.scriptableFlower;
            for (int j = 0; j < numberOfTriple; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    yield return new WaitForSeconds(0.01f);

                    TileInfo tileInfo = _tilesPool.Get();
                    tileInfo.transform.localPosition = new Vector3(
                        Random.Range(-5f, 5f),
                        Random.Range(0f, 8f),
                        Random.Range(-5f, 5f)
                    );
                    tileInfo.transform.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);
                    tileInfo.UpdateScriptableFlower(scriptableFlower);
                    _tileInfoList.Add(tileInfo);
                }
            }
        }
        ChangeState(LevelState.DropTiles);
    }

    private void HandleDropTiles()
    {
        _tileInfoList.ForEach(
            (tileInfo) =>
            {
                tileInfo.ChangeState(TileState.Drop);
            }
        );
    }

    public IEnumerator<WaitForSeconds> MatchTiles(List<TileInfo> matchTileInfos)
    {
        _ingameScreen.AddStar(matchTileInfos[1].transform.position);

        yield return new WaitForSeconds(0.5f);
        matchTileInfos.ForEach(
            (tileInfo) =>
            {
                _tileInfoList.Remove(tileInfo);
                _tilesPool.Release(tileInfo);
            }
        );
    }
}

public enum LevelState
{
    SpawnTiles = 0,
    DropTiles = 1,
}
