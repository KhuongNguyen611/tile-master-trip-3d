using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TilesStackController : StaticInstance<TilesStackController>
{
    [SerializeField]
    private Transform _stackBgs;

    [SerializeField]
    private Transform _stackTiles;

    private List<Transform> _listBGs = new();

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
}
