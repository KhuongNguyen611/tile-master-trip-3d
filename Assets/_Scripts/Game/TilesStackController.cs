using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesStackController : StaticInstance<TilesStackController>
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

    public void ShowStack()
    {
        _stackBgs.gameObject.SetActive(true);
    }

    public void AddTile(TileInfo newTile)
    {
        newTile.transform.parent = _stackTiles.transform;

        _listTileInfos.Add(newTile);

        _listTileInfos.Sort(
            (t1, t2) => t1.ScriptableFlower.flowerID.CompareTo(t2.ScriptableFlower.flowerID)
        );

        newTile.transform.DOScale(Vector3.one * 0.7f, 0.5f);
        newTile.transform.DORotate(Helpers.CheckRotation(newTile.transform.eulerAngles), 0.5f);

        StartCoroutine(
            SortStack(() =>
            {
                StartCoroutine(CheckMatch(newTile));
            })
        );
    }

    private IEnumerator SortStack(Action callback = null)
    {
        for (int i = 0; i < _listTileInfos.Count; i++)
        {
            TileInfo currentTile = _listTileInfos[i];
            Vector3 tilePosition = _listBGs[i].transform.position;
            currentTile.transform.DOMove(tilePosition, 0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    private IEnumerator CheckMatch(TileInfo newTile)
    {
        List<TileInfo> equalTilesList = _listTileInfos
            .FindAll(t => t.ScriptableFlower.flowerID.Equals(newTile.ScriptableFlower.flowerID))
            .ToList();

        if (equalTilesList.Count == 3)
        {
            equalTilesList.ForEach(tile =>
            {
                _listTileInfos.Remove(tile);

                tile.transform.DOScale(Vector3.zero, 0.5f);
            });

            StartCoroutine(LevelManager.Instance.MatchTiles(equalTilesList));

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(SortStack());
        }
    }
}
