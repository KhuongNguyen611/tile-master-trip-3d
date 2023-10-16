using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : View
{
    [SerializeField]
    private TextMeshProUGUI _levelNumberTmp;

    [SerializeField]
    private Button _playButton;

    public override void Init()
    {
        ScriptablePlayerProgress playerProgress = ResourceSystem.Instance.PlayerProgress;
        _levelNumberTmp.text = playerProgress.currentLevel.ToString();
        _playButton.onClick.AddListener(() => GameManager.Instance.StartGame());
    }
}
