using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "ScriptableObject/StepGenerator")]
public class StepGenerator : MySystem
{
    [SerializeField] private List<GameObject> _stepPrefab;
    [SerializeField] private GameObject _startStep;
    [SerializeField] private GameObject _finalStep;
    [SerializeField] private int _stepNumber;
    [SerializeField] private TransformEvent _onLevelSpawn;
    [SerializeField] private SimpleEvent _onGameStart;
    [SerializeField] private IntVariable _currentStepMaterial;
    [SerializeField] private IntVariable _totalSteps;
    [SerializeField] private SimpleEvent _onLevelEnd;
    [SerializeField] private IntVariable _currentLevel;
    [SerializeField] private IntVariable _nextLevel;
    [SerializeField] private MeshHandler _meshHandler;
    private int _lastStepIndex;
    private Transform _parent;
    private GameObject _startStepChild;


    public override void Initialize()
    {
        base.Initialize();
        _stepNumber = 10;
        _totalSteps.Value = _stepNumber;
        _currentLevel.Value = 1;
        _nextLevel.Value = 2;
        onSystemInitialized.Invoke();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _onLevelSpawn.Subscribe(SpawnSteps);
        _onLevelEnd.Subscribe(RespawnSteps);
        _onGameStart.Subscribe(StartGame);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _onLevelSpawn.Unsubscribe(SpawnSteps);
        _onLevelEnd.Unsubscribe(RespawnSteps);
        _onGameStart.Unsubscribe(StartGame);
    }

    private void SpawnSteps(Transform level)
    {
        _parent = level;
        SpawnStep(0, _startStep);
        for (int i = 0; i < _stepNumber; i++)
        {
            SpawnStep(i+1, _stepPrefab[Random.Range(0, _stepPrefab.Count)]);
        }

        _lastStepIndex+=2;
        SpawnStep(_lastStepIndex, _finalStep);
    }
    
    private void SpawnStep(int index, GameObject step)
    {
        
        GameObject obj = Instantiate(step, new Vector3(0, index * -4, 0), Quaternion.Euler(0,Random.Range(-360.0f, 360.0f),0));
        _lastStepIndex = index;
        obj.transform.SetParent(_parent);
        foreach (Transform child in obj.transform)
        {
            if (child.gameObject.CompareTag("GameOver"))
                return;
            if(child.gameObject.CompareTag("StartStep"))
            {
                _startStepChild = child.gameObject;
            }
            MeshRenderer meshRenderer = child.gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                meshRenderer.material = _meshHandler.GetMaterial(_currentStepMaterial.Value);
            }
        }
    }

    private void RespawnSteps()
    {
        _currentLevel.Value++;
        _nextLevel.Value++;
        _lastStepIndex += 5;
        _stepNumber += 15;
        for (int i = _lastStepIndex; i < _stepNumber; i++)
        {
            SpawnStep(i, _stepPrefab[Random.Range(0, _stepPrefab.Count)]);
        }
        _lastStepIndex+=2;
        SpawnStep(_lastStepIndex, _finalStep);
    }

    private void StartGame()
    {
        BoxCollider collider = _startStepChild.GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }
}