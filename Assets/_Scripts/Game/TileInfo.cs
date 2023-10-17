using System;
using cakeslice;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileInfo
    : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
{
    public ScriptableFlower ScriptableFlower { get; private set; }

    [SerializeField]
    private SpriteRenderer _upFlowerSprite;

    [SerializeField]
    private SpriteRenderer _downFlowerSprite;

    public TileState State { get; private set; }

    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private Outline _outline;

    public void UpdateScriptableFlower(ScriptableFlower scriptableFlower)
    {
        ScriptableFlower = scriptableFlower;
        UpdateFlowerSprite(ScriptableFlower.sprite);
    }

    private void UpdateFlowerSprite(Sprite sprite)
    {
        _upFlowerSprite.sprite = sprite;
        _downFlowerSprite.sprite = sprite;
    }

    public void ChangeState(TileState newState)
    {
        State = newState;
        switch (newState)
        {
            case TileState.Drop:
                HandleDrop();
                break;
            case TileState.Ground:
                break;
            case TileState.Stack:
                HandleStack();
                break;
            case TileState.Match:
                HandleMatch();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        Debug.Log($"Tile State: {newState}");
    }

    private void HandleDrop()
    {
        _rigidbody.isKinematic = false;
        ChangeState(TileState.Ground);
    }

    private void HandleStack()
    {
        _rigidbody.isKinematic = true;
        _outline.enabled = false;
        TilesStackController.Instance.AddTile(this);
    }

    private void HandleMatch()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        switch (State)
        {
            case TileState.Ground:
                if (_outline.enabled)
                {
                    Vector3 currentRotation = transform.eulerAngles;
                    currentRotation.x = Helpers.CheckAngle(currentRotation.x);
                    currentRotation.z = Helpers.CheckAngle(currentRotation.z);
                    transform.DORotate(currentRotation, 0.5f);
                }
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (State)
        {
            case TileState.Ground:
                _outline.enabled = true;
                _rigidbody.isKinematic = true;
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (State)
        {
            case TileState.Ground:
                _outline.enabled = false;
                _rigidbody.isKinematic = false;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (State)
        {
            case TileState.Ground:
                ChangeState(TileState.Stack);
                break;
        }
    }
}

public enum TileState
{
    Drop = 0,

    Ground = 1,
    Stack = 2,
    Match = 3
}
