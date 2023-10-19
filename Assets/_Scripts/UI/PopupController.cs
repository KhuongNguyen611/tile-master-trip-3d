using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : StaticInstance<PopupController>
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

    [SerializeField]
    private RectTransform _boardRect;

    public PopupState State { get; private set; }

    [SerializeField]
    private GameObject _resultDisplay;

    [SerializeField]
    private GameObject _soundDisplay;

    [SerializeField]
    private Toggle _soundToggle;

    void OnEnable()
    {
        Vector2 anchorPos = _boardRect.anchoredPosition;
        anchorPos.y += 50;
        _boardRect.anchoredPosition = anchorPos;
        _boardRect.DOAnchorPosY(_boardRect.anchoredPosition.y - 50, 0.25f);
        _homeButton.gameObject.SetActive(true);
    }

    void Start()
    {
        _homeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            ViewManager.Instance.Show<HomeScreen>(false);
            LevelManager.Instance.ChangeState(LevelState.Hide);
            AudioSystem.Instance.PlaySFX("ClickButton");
        });
        _confirmButton.onClick.AddListener(() =>
        {
            AudioSystem.Instance.PlaySFX("ClickButton");
            switch (State)
            {
                case PopupState.Win:
                case PopupState.Lose:
                    gameObject.SetActive(false);
                    LevelManager.Instance.ChangeState(LevelState.Restart);

                    break;
                case PopupState.Pause:
                    gameObject.SetActive(false);
                    break;
            }
        });
        _soundToggle.onValueChanged.AddListener(
            (isOn) =>
            {
                AudioSystem.Instance.PlaySFX("ClickButton");
                AudioSystem.Instance.SetMuteSFX(isOn);
            }
        );
        gameObject.SetActive(false);
    }

    public void ChangeState(PopupState newState)
    {
        State = newState;
        switch (newState)
        {
            case PopupState.Win:
                HandleWin();
                break;
            case PopupState.Lose:
                HandleLose();
                break;
            case PopupState.Pause:
                HandlePause();
                break;
            case PopupState.Setting:
                HandleSetting();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        Debug.Log($"Level State: {newState}");
    }

    private void HandleWin()
    {
        _resultTitleTmp.text = "You Win";
        _displayImage.sprite = _winSprite;
        _confirmButtonTmp.text = "Next Level";
    }

    private void HandleLose()
    {
        _resultTitleTmp.text = "You Lose";
        _displayImage.sprite = _loseSprite;
        _confirmButtonTmp.text = "Try Again";
    }

    private void HandlePause()
    {
        gameObject.SetActive(true);
        _bannerTitleTmp.text = "Pause";
        _resultTitleTmp.gameObject.SetActive(false);
        _resultDisplay.SetActive(false);
        _soundDisplay.SetActive(true);
        _confirmButtonTmp.text = "Continue";
    }

    private void HandleSetting()
    {
        ChangeState(PopupState.Pause);
        _bannerTitleTmp.text = "Setting";
        _homeButton.gameObject.SetActive(false);
        _confirmButtonTmp.text = "Done";
    }

    public void UpdateResult(int number)
    {
        gameObject.SetActive(true);
        ScriptablePlayerProgress playerProgress = ResourceSystem.Instance.PlayerProgress;
        _bannerTitleTmp.text = "Level " + playerProgress.currentLevel.ToString();
        _resultTitleTmp.gameObject.SetActive(true);

        string numberString = number.ToString();
        if (number == -1)
        {
            ChangeState(PopupState.Lose);
        }
        else
        {
            ChangeState(PopupState.Win);
            numberString = "+" + numberString;
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

        _resultDisplay.SetActive(true);
        _soundDisplay.SetActive(false);
    }
}

public enum PopupState
{
    Win,
    Lose,
    Pause,
    Setting,
}
