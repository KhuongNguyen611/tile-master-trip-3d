using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// One repository for all scriptable objects. Create your query methods here to keep your business logic clean.
/// I make this a MonoBehaviour as sometimes I add some debug/development references in the editor.
/// If you don't feel free to make this a standard class
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem>
{
    public List<ScriptableLevel> Levels { get; private set; }

    public ScriptablePlayerProgress PlayerProgress;

    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources()
    {
        Levels = Resources.LoadAll<ScriptableLevel>("Levels").ToList();
        PlayerProgress = Resources.Load<ScriptablePlayerProgress>("ScriptablePlayerProgress");
    }

    public ScriptableLevel GetCurrentLevel() => Levels[PlayerProgress.currentLevel];
}
