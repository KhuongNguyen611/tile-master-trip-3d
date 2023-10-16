using System.Collections.Generic;
using cakeslice;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileInfo
    : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerExitHandler,
        IPointerEnterHandler
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

    private bool _isOnTheGround;

    private void UpdateFlowerSprite(Sprite sprite)
    {
        _upFlowerSprite.sprite = sprite;
        _downFlowerSprite.sprite = sprite;
    }

    public void UpdateScriptableFlower(ScriptableFlower scriptableFlower)
    {
        _scriptableFlower = scriptableFlower;
        UpdateFlowerSprite(_scriptableFlower.sprite);
        _isOnTheGround = false;
    }

    public void Drop()
    {
        _rigidbody.isKinematic = false;
        _isOnTheGround = true;
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData) { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isOnTheGround)
            return;

        _outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isOnTheGround)
            return;

        _outline.enabled = false;
    }
}
