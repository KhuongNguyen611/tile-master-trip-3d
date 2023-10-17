using System;
using System.Collections.Generic;
using System.Transactions;
using cakeslice;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class TileInfo
    : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
{
    [SerializeField]
    private SpriteRenderer _upFlowerSprite;

    [SerializeField]
    private SpriteRenderer _downFlowerSprite;

    [SerializeField]
    private Outline _outline;

    [SerializeField]
    private Rigidbody _rigidbody;

    private ScriptableFlower _scriptableFlower;

    public TileState State { get; private set; }

    private void UpdateFlowerSprite(Sprite sprite)
    {
        _upFlowerSprite.sprite = sprite;
        _downFlowerSprite.sprite = sprite;
    }

    public void UpdateScriptableFlower(ScriptableFlower scriptableFlower)
    {
        _scriptableFlower = scriptableFlower;
        UpdateFlowerSprite(_scriptableFlower.sprite);
    }

    public void ChangeState(TileState newState)
    {
        State = newState;
        switch (newState)
        {
            case TileState.Drop:
                HandleDrop();
                break;
            case TileState.Stack:
                HandleStack();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        Debug.Log($"Tile State: {newState}");
    }

    private void HandleDrop()
    {
        _rigidbody.isKinematic = false;
    }

    private void HandleStack()
    {
        _rigidbody.isKinematic = true;
        _outline.enabled = false;
        TilesStackController.Instance.AddTile(this);
    }

    void Update()
    {
        switch (State)
        {
            case TileState.Drop:
                if (Mathf.Approximately(_rigidbody.velocity.sqrMagnitude, 0f))
                {
                    ChangeState(TileState.Ground);
                }
                break;
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
    Stack = 2
}
