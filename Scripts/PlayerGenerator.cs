using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerGenerator")]
 
public class PlayerGenerator : MySystem
{
    [SerializeField] private GameObject _player;
    [SerializeField] private TransformEvent _onPlayerSpawn;
    public override void Initialize()
    {
        base.Initialize();
        SpawnPlayer();
        onSystemInitialized.Invoke();
    }

    private void SpawnPlayer()
    {
        _onPlayerSpawn.Invoke(Instantiate(_player, new Vector3(0,0,-1), Quaternion.identity).transform);
    }
}
