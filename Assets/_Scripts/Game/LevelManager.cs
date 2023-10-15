using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class LevelManager : StaticInstance<LevelManager>
{
    [SerializeField]
    private GameObject _tileParent;
}
