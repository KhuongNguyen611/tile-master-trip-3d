using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FlowerID
{
    Flower_000,
    Flower_001,
    Flower_002,
    Flower_003,
    Flower_004,
    Flower_005,
    Flower_006,
    Flower_007,
    Flower_008,
    Flower_009,
    Flower_010,
    Flower_011,
    Flower_012,
    Flower_013,
    Flower_014,
    Flower_015,
}

[CreateAssetMenu(fileName = "FlowerSO", menuName = "Scriptable Object/Flower")]
public class FlowerSO : ScriptableObject
{
    public FlowerID tileID;

    public Sprite sprite;
}
