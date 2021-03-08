using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CameraGenerator")]
public class CameraGenerator : MySystem
{
   [SerializeField] private TransformEvent _onPlayerSpawn;
   [SerializeField] private GameObject _cameraObject;
   
   public override void Initialize()
   {
      base.Initialize();
      SpawnCamera();
      onSystemInitialized.Invoke();
   }

   protected override void OnEnable()
   {
      base.OnEnable();
      _onPlayerSpawn.Subscribe(SetTarget);
   }

   protected override void OnDisable()
   {
      base.OnDisable();
      _onPlayerSpawn.Unsubscribe(SetTarget);
   }

   private void SpawnCamera()
   {
      Instantiate(_cameraObject);
   }

   private void SetTarget(Transform player)
   {
      Camera.main.GetComponent<MoveCamera>().Player = player.gameObject;
   }
}
