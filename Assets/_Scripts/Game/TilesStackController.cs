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

    [SerializeField]
    private Transform _stackTiles;

    private List<Transform> _listBGs = new();

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

    public void AddTile(TileInfo tileInfo)
    {
        _listTileInfos.Add(tileInfo);
        tileInfo.transform.parent = _stackTiles.transform;
        Vector3 tilePosition = _listBGs[_listTileInfos.Count - 1].transform.position;
        tileInfo.transform.DOMove(tilePosition, 0.5f);
        tileInfo.transform.DOScale(Vector3.one * 0.7f, 0.5f);
        tileInfo.transform.DORotate(Helpers.CheckRotation(tileInfo.transform.eulerAngles), 0.5f);
    }
}
