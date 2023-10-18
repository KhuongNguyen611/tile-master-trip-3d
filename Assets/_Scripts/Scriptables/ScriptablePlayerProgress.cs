using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptablePlayerProgress", menuName = "Scriptable Object/PlayerInfo")]
public class ScriptablePlayerProgress : ScriptableObject
{
    [HideInInspector]
    public bool IsCompletedTutorial => !currentLevel.Equals(0);

    [HideInInspector]
    public int TotalStar;

    public int currentLevel;
}
