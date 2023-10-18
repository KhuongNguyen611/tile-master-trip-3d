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

    [SerializeField]
    private TextMeshProUGUI _starAmountTmp;

    void OnEnable()
    {
        ScriptablePlayerProgress playerProgress = ResourceSystem.Instance.PlayerProgress;
        _levelNumberTmp.text = playerProgress.currentLevel.ToString();
        _starAmountTmp.text = playerProgress.TotalStar.ToString();
    }

    public override void Init()
    {
        _playButton.onClick.AddListener(
            () => GameManager.Instance.ChangeState(GameState.StartLevel)
        );
    }
}
