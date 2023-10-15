using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _flowerSprite;

    [SerializeField]
    private SpriteRenderer _chooseSprite;

    [SerializeField]
    private ScriptableFlower _scriptableFlower;

    void Start()
    {
        _flowerSprite.sprite = _scriptableFlower.sprite;
    }

    public void UpdateScriptableFlower(ScriptableFlower scriptableFlower)
    {
        _scriptableFlower = scriptableFlower;
        _flowerSprite.sprite = _scriptableFlower.sprite;
    }
}
