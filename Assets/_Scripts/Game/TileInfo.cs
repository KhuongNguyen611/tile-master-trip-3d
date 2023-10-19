using System;
using System.Collections.Generic;
using cakeslice;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

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

    [SerializeField]
    private List<CapsuleCollider> _listCapsuleColliders;

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
            case TileState.Spawn:
                HandleSpawn();
                break;
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

    private void HandleSpawn()
    {
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
    }

    private void HandleDrop()
    {
        _listCapsuleColliders.ForEach(
            (collider) =>
            {
                collider.enabled = true;
            }
        );
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        ChangeState(TileState.Ground);
    }

    private void HandleStack()
    {
        _listCapsuleColliders.ForEach(
            (collider) =>
            {
                collider.enabled = false;
            }
        );
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        _outline.enabled = false;
        LevelManager.Instance.AddTile(this);
    }

    private void HandleMatch()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
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
    Spawn,
    Drop,
    Ground,
    Stack,
    Match,
}
