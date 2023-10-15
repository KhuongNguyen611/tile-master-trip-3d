using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlowerAmount
{
    public ScriptableFlower scriptableFlower;

    public int numberOfTriples = 1;
}

[CreateAssetMenu(fileName = "ScriptableLevel", menuName = "Scriptable Object/Level")]
public class ScriptableLevel : ScriptableObject
{
    public List<FlowerAmount> flowerAmountList;
}
