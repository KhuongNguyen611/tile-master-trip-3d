using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableLevel", menuName = "Scriptable Object/Level")]
public class ScriptableLevel : ScriptableObject
{
    public List<FlowerAmount> flowerAmountList;
}

[Serializable]
public class FlowerAmount
{
    public ScriptableFlower scriptableFlower;

    public int numberOfTriples = 1;
}
