using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FlowerID
{
    Flower_0,
    Flower_1,
    Flower_2,
    Flower_3,
    Flower_4,
    Flower_5,
    Flower_6,
    Flower_7,
    Flower_8,
    Flower_9,
    Flower_10,
    Flower_11,
    Flower_12,
    Flower_13,
    Flower_14,
    Flower_15,
}

[CreateAssetMenu(fileName = "ScriptableFlower", menuName = "Scriptable Object/Flower")]
public class ScriptableFlower : ScriptableObject
{
    public FlowerID flowerID;

    public Sprite sprite;
}
