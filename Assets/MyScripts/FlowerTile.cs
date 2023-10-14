using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTile : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer flowerSprite;

    private FlowerSO flowerSO;

    public void UpdateFlowerSO(FlowerSO initSO)
    {
        flowerSO = initSO;
        flowerSprite.sprite = flowerSO.sprite;
    }
}
