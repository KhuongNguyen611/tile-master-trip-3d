using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControl : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _flowerSprite;

    private ScriptableFlower _scriptableFlower;

    public void UpdateFlowerSO(ScriptableFlower scriptableFlower)
    {
        _scriptableFlower = scriptableFlower;
        _flowerSprite.sprite = _scriptableFlower.sprite;
    }
}
