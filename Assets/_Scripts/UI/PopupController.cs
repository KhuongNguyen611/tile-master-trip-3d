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

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
