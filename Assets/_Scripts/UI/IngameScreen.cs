using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

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
    private RectTransform _starRectOnBar;

    private ObjectPool<RectTransform> _starPool;

    [SerializeField]
    private GameObject _comboBar;

    private int _comboCount = 0;

    private float _comboTimer = 0;

    private float _maxTimer = 7;

    private float _currentMaxTimer;

    [SerializeField]
    private TextMeshProUGUI _comboTmp;

    [SerializeField]
    private RectTransform _comboProgressBar;

    public override void Init()
    {
        _starAmount = 0;
        _starTmp.text = _starAmount.ToString();
        ScriptablePlayerProgress playerProgress = ResourceSystem.Instance.PlayerProgress;
        _levelTmp.text = "Lv." + playerProgress.currentLevel.ToString();

        _starPool = new ObjectPool<RectTransform>(
            () =>
            {
                return Instantiate(_starRectOnBar.gameObject, transform)
                    .GetComponent<RectTransform>();
            },
            star =>
            {
                star.gameObject.SetActive(true);
            },
            star =>
            {
                star.gameObject.SetActive(false);
            },
            star =>
            {
                Destroy(star);
            },
            false,
            6,
            10
        );
    }

    public void AddStar(Vector3 objectPosition)
    {
        Vector2 viewportPosition = _camera.WorldToViewportPoint(objectPosition);
        Vector2 objectScreenPosition = new Vector2(
            ((viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f))
        );

        _comboBar.SetActive(true);

        _comboCount++;
        _currentMaxTimer = _maxTimer - _comboCount;
        _comboTimer = _currentMaxTimer;
        _comboTmp.text = "Combo x" + _comboCount.ToString();

        var sequence = DOTween.Sequence();
        for (int i = 0; i < _comboCount; i++)
        {
            sequence.AppendCallback(() =>
            {
                SpawnStar(objectScreenPosition);
            });
            sequence.AppendInterval(0.1f);
        }
    }

    private void SpawnStar(Vector2 screenPosition)
    {
        RectTransform starRect = _starPool.Get().GetComponent<RectTransform>();
        starRect.anchoredPosition = screenPosition;

        Vector3 bouncePosition = starRect.position;
        float scale = bouncePosition.x / bouncePosition.y;
        float addDistance = 20f;
        bouncePosition.x += addDistance;
        bouncePosition.y -= addDistance * scale;

        var sequence = DOTween.Sequence();
        sequence.Append(starRect.DOMove(bouncePosition, 0.5f));
        sequence.Append(starRect.DOMove(_starRectOnBar.position, 0.25f));
        sequence.AppendCallback(() =>
        {
            _starAmount++;
            _starTmp.text = _starAmount.ToString();
            _starPool.Release(starRect);
        });
    }

    void Update()
    {
        if (_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;

            float percent = _comboTimer / _currentMaxTimer;

            _comboProgressBar.localScale = new Vector2(percent, 1);

            if (_comboTimer < 0)
            {
                _comboCount = 0;
                _comboBar.SetActive(false);
            }
        }
    }
}
