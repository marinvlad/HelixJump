using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/LevelGenerator")]
public class LevelGenerator : MySystem
{
    [SerializeField] private GameObject _object;
    [SerializeField] private TransformEvent _onLevelSpawnEvent;
    [SerializeField] private SimpleEvent _onRestart;
    private GameObject _instance;
    public override void Initialize()
    {
        base.Initialize();
        SpawnLevel();
        onSystemInitialized.Invoke();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _onRestart.Subscribe(Restart);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _onRestart.Unsubscribe(Restart);
    }

    private void SpawnLevel()
    {
        _instance = Instantiate(_object, new Vector3(0, 0, 0), Quaternion.identity);
        _onLevelSpawnEvent.Invoke(_instance.transform);
    }

    private void Restart()
    {
        DestroyImmediate(_instance);
        SpawnLevel();
    }
}
