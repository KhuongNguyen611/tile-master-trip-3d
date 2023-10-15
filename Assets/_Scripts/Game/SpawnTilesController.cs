using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnTilesController : StaticInstance<SpawnTilesController>
{
    [SerializeField]
    private GameObject _flowerTilePrefab;

    private List<TileInfo> _tileInfoList = new();

    public void SpawnTiles()
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
            flowerTile.transform.parent = transform;
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
}
