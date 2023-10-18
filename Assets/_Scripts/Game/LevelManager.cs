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

    private List<TileInfo> _listTileInfos = new();

    [SerializeField]
    private IngameScreen _ingameScreen;

    private ObjectPool<TileInfo> _poolTiles;

    [SerializeField]
    private TilesStackController _tileStackController;

    [SerializeField]
    private PopupController _popupController;

    void Start()
    {
        _poolTiles = new ObjectPool<TileInfo>(
            () =>
            {
                return Instantiate(_flowerTilePrefab, _tilesParent.transform)
                    .GetComponent<TileInfo>();
            },
            null,
            tileInfo =>
            {
                tileInfo.gameObject.SetActive(false);
                tileInfo.transform.position = Vector3.zero;
            },
            tileInfo =>
            {
                Destroy(tileInfo.gameObject);
            },
            false,
            50,
            100
        );

        _tilesParent.SetActive(false);
        _tileStackController.gameObject.SetActive(false);
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
            case LevelState.Win:
                HandleWin();
                break;
            case LevelState.Lose:
                HandleLose();
                break;
            case LevelState.Hide:
                HandleHide();
                break;
            case LevelState.Restart:
                HandleRestart();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        Debug.Log($"Level State: {newState}");
    }

    private void HandleSpawnTiles()
    {
        ViewManager.Instance.Show<IngameScreen>();

        _tileStackController.gameObject.SetActive(true);

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

                    TileInfo tileInfo = _poolTiles.Get();
                    tileInfo.transform.parent = _tilesParent.transform;
                    tileInfo.transform.localPosition = new Vector3(
                        Random.Range(-5f, 5f),
                        Random.Range(0f, 8f),
                        Random.Range(-5f, 5f)
                    );
                    tileInfo.ChangeState(TileState.Spawn);
                    tileInfo.transform.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);
                    tileInfo.UpdateScriptableFlower(scriptableFlower);
                    _listTileInfos.Add(tileInfo);
                }
            }
        }
        ChangeState(LevelState.DropTiles);
    }

    private void HandleDropTiles()
    {
        _tilesParent.SetActive(true);

        _listTileInfos.ForEach(
            (tileInfo) =>
            {
                tileInfo.ChangeState(TileState.Drop);
            }
        );
    }

    public void AddTile(TileInfo tileInfo)
    {
        _tileStackController.AddTile(tileInfo);
    }

    public IEnumerator<WaitForSeconds> MatchTiles(List<TileInfo> matchTileInfos)
    {
        _ingameScreen.AddStar(matchTileInfos[1].transform.position);

        yield return new WaitForSeconds(0.5f);
        matchTileInfos.ForEach(
            (tileInfo) =>
            {
                _listTileInfos.Remove(tileInfo);
                _poolTiles.Release(tileInfo);
            }
        );

        if (_listTileInfos.Count == 0)
        {
            ChangeState(LevelState.Win);
        }
    }

    private void HandleWin()
    {
        _popupController.gameObject.SetActive(true);
        ScriptablePlayerProgress playerProgress = ResourceSystem.Instance.PlayerProgress;
        playerProgress.TotalStar += _ingameScreen.StarAmount;
        playerProgress.currentLevel++;
        _popupController.UpdateResult(_ingameScreen.StarAmount);
        StopAllCoroutines();
    }

    private void HandleLose()
    {
        _popupController.gameObject.SetActive(true);
        _popupController.UpdateResult(-1);
        StopAllCoroutines();
    }

    private void HandleHide()
    {
        Reset();
        _tilesParent.SetActive(false);
        _tileStackController.gameObject.SetActive(false);
    }

    private void HandleRestart()
    {
        Reset();
        _tilesParent.SetActive(false);
        ChangeState(LevelState.SpawnTiles);
    }

    private void Reset()
    {
        _listTileInfos.ForEach(
            (tileInfo) =>
            {
                _poolTiles.Release(tileInfo);
            }
        );
        _listTileInfos.Clear();

        _ingameScreen.ResetUI();

        _tileStackController.ClearStack(_poolTiles);
    }
}

public enum LevelState
{
    SpawnTiles,
    DropTiles,
    Win,
    Lose,
    Hide,
    Restart
}
