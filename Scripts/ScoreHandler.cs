using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ScoreHandler")]
public class ScoreHandler : MySystem
{
   [SerializeField] private IntVariable _bestScore;
   [SerializeField] private IntVariable _lastScore;
   [SerializeField] private SimpleEvent _onQuit;
   [SerializeField] private SimpleEvent _onGameOver;
   [SerializeField] private IntEvent _onPlayerMaterialReward;

   public override void Initialize()
   {
      base.Initialize();
      GetScore();
   }

   protected override void OnEnable()
   {
      base.OnEnable();
      _onGameOver.Subscribe(GetReward);
      _onQuit.Subscribe(SaveScore);
   }

   protected override void OnDisable()
   {
      base.OnDisable();
      _onGameOver.Unsubscribe(GetReward);
      _onQuit.Unsubscribe(SaveScore);
   }

   private void GetReward()
   {
      if (_lastScore.Value> 200 && _lastScore.Value<300)
      {
         Debug.Log("ScoreHandler.cs -> 300 score received");
         _onPlayerMaterialReward.Invoke(1);
      }else if (_lastScore.Value> 500 && _lastScore.Value<600)
      {
         Debug.Log("ScoreHandler.cs -> 300 score received");
         _onPlayerMaterialReward.Invoke(2);
      } else if (_lastScore.Value> 800 && _lastScore.Value<1000)
      {
         Debug.Log("ScoreHandler.cs -> 300 score received");
         _onPlayerMaterialReward.Invoke(3);
      } else if (_lastScore.Value> 1500 && _lastScore.Value<2000)
      {
         Debug.Log("ScoreHandler.cs -> 300 score received");
         _onPlayerMaterialReward.Invoke(4);
      } else if (_lastScore.Value> 2000 && _lastScore.Value<3000)
      {
         Debug.Log("ScoreHandler.cs -> 300 score received");
         _onPlayerMaterialReward.Invoke(5);
      } else if (_lastScore.Value> 3000 && _lastScore.Value<5000)
      {
         Debug.Log("ScoreHandler.cs -> 300 score received");
         _onPlayerMaterialReward.Invoke(6);
      }else if (_lastScore.Value> 5000 && _lastScore.Value<8000)
      {
         Debug.Log("ScoreHandler.cs -> 300 score received");
         _onPlayerMaterialReward.Invoke(7);
      }
      
   }

   private void SaveScore()
   {
      PlayerPrefs.SetInt("BestScore", _bestScore.Value);
      Debug.Log("ScoreHandler.cs -> Best score saved");
   }

   private void GetScore()
   {
      int bestScore = PlayerPrefs.GetInt("BestScore");
      _bestScore.Value = bestScore;
      onSystemInitialized.Invoke();
      Debug.Log("ScoreHandler.cs -> Best score restored");
   }
}
