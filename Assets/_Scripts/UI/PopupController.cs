using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _bannerTitleTmp;

    [SerializeField]
    private TextMeshProUGUI _resultTitleTmp;

    [SerializeField]
    private TextMeshProUGUI _numberTmp;

    [SerializeField]
    private Image _displayImage;

    [SerializeField]
    private Sprite _winSprite;

    [SerializeField]
    private Sprite _loseSprite;

    [SerializeField]
    private Button _homeButton;

    [SerializeField]
    private Button _confirmButton;

    [SerializeField]
    private TextMeshProUGUI _confirmButtonTmp;

    void Awake()
    {
        ScriptablePlayerProgress playerProgress = ResourceSystem.Instance.PlayerProgress;
        _bannerTitleTmp.text = "Level " + playerProgress.currentLevel.ToString();
    }

    void Start()
    {
        _homeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            ViewManager.Instance.Show<HomeScreen>(false);
            LevelManager.Instance.ChangeState(LevelState.Hide);
        });
        _confirmButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            LevelManager.Instance.ChangeState(LevelState.Restart);
        });
    }

    public void UpdateResult(int number)
    {
        string numberString = number.ToString();
        if (number == -1)
        {
            _resultTitleTmp.text = "You Lose";
            _displayImage.sprite = _loseSprite;
            _confirmButtonTmp.text = "Try Again";
        }
        else
        {
            _resultTitleTmp.text = "You Win";
            numberString = "+" + numberString;
            _displayImage.sprite = _winSprite;
            _confirmButtonTmp.text = "Next Level";
        }

        _numberTmp.text = numberString;

        if (numberString.Length >= 3)
        {
            _numberTmp.horizontalAlignment = HorizontalAlignmentOptions.Right;
        }
        else
        {
            _numberTmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
        }
    }
}
