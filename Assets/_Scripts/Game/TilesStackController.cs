using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

public class TilesStackController : MonoBehaviour
{
    [SerializeField]
    private Transform _stackBgs;
    private List<Transform> _listBGs = new();

    [SerializeField]
    private Transform _stackTiles;

    private List<TileInfo> _listTileInfos = new();

    void Start()
    {
        foreach (Transform bg in _stackBgs)
        {
            _listBGs.Add(bg);
        }
    }

    public void AddTile(TileInfo newTile)
    {
        if (_listTileInfos.Count >= _listBGs.Count - 1)
            return;

        AudioSystem.Instance.PlaySFX("PickTile");

        newTile.transform.parent = _stackTiles.transform;

        _listTileInfos.Add(newTile);

        _listTileInfos.Sort(
            (t1, t2) => t1.ScriptableFlower.flowerID.CompareTo(t2.ScriptableFlower.flowerID)
        );

        newTile.transform.DOScale(Vector3.one * 0.7f, 0.5f);
        newTile.transform.DORotate(Helpers.CheckRotation(newTile.transform.eulerAngles), 0.5f);

        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            SortStack();
        });
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() =>
        {
            CheckMatch(newTile);
        });
    }

    private void SortStack()
    {
        for (int i = 0; i < _listTileInfos.Count; i++)
        {
            TileInfo currentTile = _listTileInfos[i];
            Vector3 tilePosition = _listBGs[i].transform.position;
            currentTile.transform.DOMove(tilePosition, 0.5f);
        }
    }

    private void CheckMatch(TileInfo newTile)
    {
        int index = _listTileInfos.FindIndex(t => t == newTile);
        List<TileInfo> subList = _listTileInfos.GetRange(0, index + 1);
        subList = subList.FindAll(
            t => t.ScriptableFlower.flowerID.Equals(newTile.ScriptableFlower.flowerID)
        );

        var sequence = DOTween.Sequence();
        if (subList.Count == 3)
        {
            subList.ForEach(tile =>
            {
                _listTileInfos.Remove(tile);
                tile.transform
                    .DOScale(Vector3.zero, 0.5f)
                    .OnComplete(() =>
                    {
                        tile.ChangeState(TileState.Match);
                    });
            });
            StartCoroutine(LevelManager.Instance.MatchTiles(subList));
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                SortStack();
            });
            sequence.AppendInterval(0.5f);
        }

        sequence.AppendCallback(() =>
        {
            if (_listTileInfos.Count >= _listBGs.Count - 1)
            {
                if (LevelManager.Instance.State != LevelState.Lose)
                {
                    LevelManager.Instance.ChangeState(LevelState.Lose);
                }
            }
        });
    }

    public void ClearStack(ObjectPool<TileInfo> poolTiles)
    {
        // _listTileInfos.ForEach(
        //     (tileInfo) =>
        //     {
        //         poolTiles.Release(tileInfo);
        //     }
        // );
        _listTileInfos.Clear();
    }
}
