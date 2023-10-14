using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelId
{
    ShowLevelPanel,
    PausePanel,
    ResultPanel
}

[Serializable]
public class PanelModel
{
    public PanelId panelId;

    public GameObject PanelPrefab;
}
