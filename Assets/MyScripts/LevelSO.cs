using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FlowerAmount
{
    public FlowerSO flowerSO;

    public int numberOfTriple;
}

[CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Object/Level")]
public class LevelSO : ScriptableObject
{
    public List<FlowerAmount> flowerAmountsList;
}
