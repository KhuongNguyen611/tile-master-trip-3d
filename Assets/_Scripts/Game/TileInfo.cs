using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _upFlowerSprite;

    [SerializeField]
    private SpriteRenderer _downFlowerSprite;

    [SerializeField]
    private SpriteRenderer _chooseSprite;

    [SerializeField]
    private ScriptableFlower _scriptableFlower;

    void Start()
    {
        UpdateFlowerSprite(_scriptableFlower.sprite);
    }

    void UpdateFlowerSprite(Sprite sprite)
    {
        _upFlowerSprite.sprite = sprite;
        _downFlowerSprite.sprite = sprite;
    }

    public void UpdateScriptableFlower(ScriptableFlower scriptableFlower)
    {
        _scriptableFlower = scriptableFlower;
        UpdateFlowerSprite(_scriptableFlower.sprite);
    }
}
