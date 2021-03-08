using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/MeshHandler")]
public class MeshHandler : MySystem
{
    public AvailableMeshes _availableMeshes;
    public List<int> _playerMeshes;
    public IntVariable _currentMesh;
    public SimpleEvent _onQuitEvent;
    public SimpleEvent _onRestart;
    public SimpleEvent _onPlayerDataLoad;
    public IntEvent _onPlayerMaterialReward;
    public IntEvent _onNewPlayerMaterialReward;
    public override void Initialize()
    {
        base.Initialize();
        GetMeshes();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerMeshes = new List<int>();
        _onQuitEvent.Subscribe(SaveMeshes);
        _onPlayerMaterialReward.Subscribe(AddMeshToPlayer);
        _currentMesh.onValueChanged += SaveMeshes;
        _currentMesh.onValueChanged += ResetStepMaterial;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _onQuitEvent.Unsubscribe(SaveMeshes);
        _onPlayerMaterialReward.Unsubscribe(AddMeshToPlayer);
        _currentMesh.onValueChanged -= SaveMeshes;
        _currentMesh.onValueChanged -= ResetStepMaterial;
    }

    private void GetMeshes()
    {
        var _noPlayerSkins = PlayerPrefs.GetInt("PlayerAvalibleSkins");
        for (var i = 0; i < _noPlayerSkins; i++)
        {
            var _skinIndex = PlayerPrefs.GetInt("PlayerAvalibleSkin" + i);
            if (!_playerMeshes.Contains(_skinIndex)) ;
            _playerMeshes.Add(_skinIndex);
        }

        if (_playerMeshes.Count == 0)
        {
            _playerMeshes.Add(0);
        }

        var restoredCurrentMesh = PlayerPrefs.GetInt("PlayerCurrentSkin");
        if (_availableMeshes.meshes[restoredCurrentMesh] != null)
        {
            _currentMesh.Value = _availableMeshes.meshes[restoredCurrentMesh].id;
        }
        else
        {
            _currentMesh.Value = 0;
        }

        _onPlayerDataLoad.Invoke();
        onSystemInitialized.Invoke();
        Debug.Log("Player data restored");
    }

    private void SaveMeshes()
    {
        if (_playerMeshes != null && _playerMeshes.Count!=0)
        {
            PlayerPrefs.SetInt("PlayerAvalibleSkins", _playerMeshes.Count);
            for (var i = 0; i < _playerMeshes.Count; i++)
                PlayerPrefs.SetInt("PlayerAvalibleSkin" + i, _playerMeshes[i]);
        }
        if(_currentMesh.Value!=null)
            PlayerPrefs.SetInt("PlayerCurrentSkin", _currentMesh.Value);
        else
        {
            _currentMesh.Value = _availableMeshes.meshes[0].id;
        }
        Debug.Log("Player data saved!");
    }

    private void ResetStepMaterial()
    {
        _onRestart.Invoke();
    }

    public int CurrentMesh
    {
        get => _currentMesh.Value;
        set => _currentMesh.Value = value;
    }

    public List<int> PlayerMeshes
    {
        get => _playerMeshes;
        set => _playerMeshes = value;
    }

    public Material GetMaterial(int id)
    {
        if (_availableMeshes.meshes[id] != null)
        {
            return _availableMeshes.meshes[id].mesh;
        }
        else
        {
            return _availableMeshes.meshes[0].mesh;
        }
    }
    

    public void AddMeshToPlayer(int id)
    {
        if (!_playerMeshes.Contains(id))
        {
            _playerMeshes.Add(id);
            _onNewPlayerMaterialReward.Invoke(id);
        }
        _onPlayerDataLoad.Invoke();
    }
}

[Serializable]
public class AvailableMeshes
{
    public List<StepMesh> meshes;
}