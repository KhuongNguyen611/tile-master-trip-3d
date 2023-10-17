using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class IngameScreen : View
{
    private int _starAmount;

    [SerializeField]
    private TextMeshProUGUI _starTmp;

    [SerializeField]
    private TextMeshProUGUI _levelTmp;

    [SerializeField]
    private GameObject _levelGO;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private RectTransform _canvasRect;

    [SerializeField]
    private RectTransform _starRectsParent;

    private List<RectTransform> _listStarRects = new();

    [SerializeField]
    private RectTransform _starRectOnBar;

    public override void Init()
    {
        _starAmount = 0;
        _starTmp.text = _starAmount.ToString();
        ScriptablePlayerProgress playerProgress = ResourceSystem.Instance.PlayerProgress;
        _levelTmp.text = "Lv." + playerProgress.currentLevel.ToString();
        _levelGO.SetActive(playerProgress.IsCompletedTutorial);
        foreach (RectTransform starRect in _starRectsParent)
        {
            _listStarRects.Add(starRect);
        }
    }

    public void AddStar(Vector3 objectPosition)
    {
        Vector2 viewportPosition = _camera.WorldToViewportPoint(objectPosition);
        Vector2 objectScreenPosition = new Vector2(
            ((viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f))
        );

        RectTransform _starRect = _listStarRects.Find(s => s.gameObject.activeInHierarchy == false);
        _starRect.anchoredPosition = objectScreenPosition;
        _starRect.gameObject.SetActive(true);

        _starRect
            .DOMove(_starRectOnBar.position, 0.5f)
            .OnComplete(() =>
            {
                _starAmount++;
                _starTmp.text = _starAmount.ToString();
                _starRect.gameObject.SetActive(false);
            });
    }
}
