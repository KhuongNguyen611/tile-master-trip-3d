using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameScreen : View
{
    [SerializeField]
    private TextMeshProUGUI _starTmp;
    private int _starAmount;

    [SerializeField]
    private TextMeshProUGUI _levelTmp;

    [SerializeField]
    private GameObject _levelGO;

    public override void Init()
    {
        _starAmount = 0;
        _starTmp.text = _starAmount.ToString();
        ScriptablePlayerProgress playerProgress = ResourceSystem.Instance.PlayerProgress;
        _levelTmp.text = "Lv." + playerProgress.currentLevel.ToString();
        _levelGO.SetActive(playerProgress.IsCompletedTutorial);
    }
}
