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

    void Update()
    {
        if (_outline.enabled)
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.x = CheckAngle(currentRotation.x);
            currentRotation.z = CheckAngle(currentRotation.z);
            transform.DORotate(currentRotation, 0.5f);
        }
    }

    private float CheckAngle(float angle)
    {
        if (angle > 0)
        {
            if (angle > -90)
            {
                angle = 0;
            }
            else
            {
                angle = -180;
            }
        }
        else
        {
            if (angle < 90)
            {
                angle = 0;
            }
            else
            {
                angle = 180;
            }
        }
        return angle;
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData) { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isOnTheGround)
            return;

        _outline.enabled = true;
        _rigidbody.isKinematic = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isOnTheGround)
            return;

        _outline.enabled = false;
        _rigidbody.isKinematic = false;
    }
}
