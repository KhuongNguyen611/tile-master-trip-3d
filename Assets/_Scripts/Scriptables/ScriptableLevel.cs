using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FlowerAmount
{
    public ScriptableFlower flowerSO;

    public int numberOfTriple;
}

[CreateAssetMenu(fileName = "ScriptableLevel", menuName = "Scriptable Object/Level")]
public class ScriptableLevel : ScriptableObject
{
    public List<FlowerAmount> flowerAmountsList;
}
