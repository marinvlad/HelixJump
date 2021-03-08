using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/UIHandler")]
public class UIHandler : MySystem
{
    [SerializeField] private GameObject _canvas;
    [BoxGroup("Menu"),SerializeField] private RectTransform _menu;
    [BoxGroup("Menu/BestScore"),SerializeField] private GameObject _bestScoreTextUI;
    [BoxGroup("Shop"),SerializeField] private RectTransform _shop;
    [BoxGroup("LevelEnd"),SerializeField] private RectTransform _levelEnd;
    [BoxGroup("TouchArea"),SerializeField] private GameObject _touchArea;
    
    [BoxGroup("GameTopSection"), SerializeField] private RectTransform _gameTopSection;
    [BoxGroup("GameTopSection"), SerializeField] private GameObject _scoreTextUI;
    [BoxGroup("GameTopSection"), SerializeField] private GameObject _currentLevelTextUI;
    [BoxGroup("GameTopSection"), SerializeField] private GameObject _nextLevelTextUI;
    [BoxGroup("GameTopSection"), SerializeField] private Slider _slider;

    [SerializeField] private IntVariable _currentLevel;
    [SerializeField] private IntVariable _nextLevel;
    
    [SerializeField] private IntVariable _currentScore;
    [SerializeField] private IntVariable _bestScore;
    
    [SerializeField] private IntVariable _currentStep;
    [SerializeField] private IntVariable _totalSteps;
    

    [BoxGroup("GameOver"), SerializeField] private SimpleEvent _onGameOverEvent;
    [BoxGroup("GameOver"), SerializeField] private SimpleEvent _onGameRestartEvent;
    [BoxGroup("GameOver"), SerializeField] private IntEvent _onNewPlayerItemReward;
    [BoxGroup("GameOver"), SerializeField] private GameObject _loadingImage;
    [BoxGroup("GameOver"), SerializeField] private GameObject _reward;

    [SerializeField] private List<Sprite> _itemsIcons;
    [SerializeField] private SimpleEvent _onGameStart;

    public override void Initialize()
    {
        base.Initialize();
        SpawnCanvas();
        onSystemInitialized.Invoke();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _currentStep.Value = 0;
        _totalSteps.Value = 10;
        _currentScore.onValueChanged += UpdateCurrentScore;
        _bestScore.onValueChanged += UpdateMaxScore;
        
        _currentLevel.onValueChanged += UpdateCurrentLevel;
        _nextLevel.onValueChanged += UpdateNextLevel;

        _currentStep.onValueChanged += UpdateStep;
        _totalSteps.onValueChanged += UpdateStep;
        
        _onGameOverEvent.Subscribe(ShowLevelEnd);
        _onNewPlayerItemReward.Subscribe(ShowRewardPopup);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _currentScore.onValueChanged -= UpdateCurrentScore;
        _bestScore.onValueChanged -= UpdateMaxScore;

        _currentLevel.onValueChanged -= UpdateCurrentLevel;
        _nextLevel.onValueChanged -= UpdateNextLevel;

        _currentStep.onValueChanged -= UpdateStep;
        _totalSteps.onValueChanged -= UpdateStep;
        
        _onGameOverEvent.Unsubscribe(ShowLevelEnd);
        _onNewPlayerItemReward.Unsubscribe(ShowRewardPopup);
    }

    public void ShowMenu()
    {
        _menu.DOAnchorPos(Vector2.zero, 0.25f);
        _gameTopSection.DOAnchorPos(new Vector2(1300, 0), 0.25f);
        _touchArea.SetActive(false);
    }

    public void StartGame()
    {
        _gameTopSection.DOAnchorPos(Vector2.zero, 0.25f);
        _menu.DOAnchorPos(new Vector2(-1300, 0), 0.25f);
        _touchArea.SetActive(true);
        UpdateStep();
        _onGameStart.Invoke();
    }

    public void ShowShop()
    {
        _shop.DOAnchorPos(new Vector2(0, 0), 0.25f);
        _scoreTextUI.SetActive(false);
    }

    public void CloseShop()
    {
        _shop.DOAnchorPos(new Vector2(0, -4500), 0.25f);
        _scoreTextUI.SetActive(true);
    }

    private void ShowLevelEnd()
    {
        _levelEnd.DOAnchorPos(new Vector2(0,0), 0.25f);
    }

    private void HideLevelEnd()
    {
        _levelEnd.DOAnchorPos(new Vector2(-2000,0), 0.25f);
    }

    public void UpdateMaxScore()
    {
        if(_bestScoreTextUI!=null)
        {
            Text text = _bestScoreTextUI.GetComponent<Text>();
            text.text = _bestScore.Value.ToString();
        }
    }

    public void UpdateCurrentScore()
    {
        if(_scoreTextUI!=null)
        {
            Text text = _scoreTextUI.GetComponent<Text>();
            text.text = _currentScore.Value.ToString();
        }
        
        if(_bestScore!=null && _currentScore!=null)
            if (_bestScore.Value < _currentScore.Value)
            {
                _bestScore.Value = _currentScore.Value;
            }
    }

    private void UpdateCurrentLevel()
    {
        _currentStep.Value = 0;
        if(_currentLevelTextUI!=null)
        {
            Text text = _currentLevelTextUI.GetComponent<Text>();
            text.text = _currentLevel.Value.ToString();
        }
    }

    private void UpdateNextLevel()
    {    
        if(_nextLevelTextUI!=null)
        {
            Text text = _nextLevelTextUI.GetComponent<Text>();
            text.text = _nextLevel.Value.ToString();
        }
    }
    
    private void UpdateStep()
    {
        if(_slider!=null)
            _slider.value =  (float)_currentStep.Value/_totalSteps.Value;
    }

    public void RestartButtonPressed()
    {
        _currentStep.Value = 0;
        _totalSteps.Value = 10;
        _currentLevel.Value = 1;
        _nextLevel.Value = 2;
        _onGameRestartEvent.Invoke();
        _reward.SetActive(false);
        HideLevelEnd();
        ShowMenu();
    }

    private void ShowRewardPopup(int id)
    {
        Image image = _loadingImage.GetComponent<Image>();
        Tween tween = image.DOFillAmount(1, 2f);
        Image itemIcon = _reward.GetComponent<Image>();
        itemIcon.sprite = _itemsIcons[id];
        tween.OnComplete(()=>
        {
            image.enabled = false;
            _reward.SetActive(true);
        });
    }
    
    private void SpawnCanvas()
    {
        GameObject _canvasInstance = Instantiate(_canvas);
        _menu = _canvasInstance.transform.Find("Menu").gameObject.GetComponent<RectTransform>();
        GameObject _menuGO = _menu.gameObject;
        GameObject _bs = _menuGO.transform.Find("BestScore").gameObject;
        _bestScoreTextUI = _bs.transform.Find("Score").gameObject;
        _shop = _canvasInstance.transform.Find("Shop").gameObject.GetComponent<RectTransform>();
        _levelEnd = _canvasInstance.transform.Find("LevelEnd").gameObject.GetComponent<RectTransform>();
        _touchArea = _canvasInstance.transform.Find("TouchArea").gameObject;
        _gameTopSection = _canvasInstance.transform.Find("GameTopSection").gameObject.GetComponent<RectTransform>();
        _scoreTextUI = _gameTopSection.transform.Find("Score").gameObject;
        GameObject _levelIndicator = _gameTopSection.gameObject.transform.Find("LevelIndicator").gameObject;
        _slider = _levelIndicator.transform.Find("Slider").gameObject.GetComponent<Slider>();
        _nextLevelTextUI = _levelIndicator.transform.Find("NextLevel").transform.Find("Text").gameObject;
        _currentLevelTextUI = _levelIndicator.transform.Find("CurrentLevel").transform.Find("Text").gameObject;
        _loadingImage = _canvasInstance.transform.Find("LevelEnd").gameObject.transform.Find("LoadingImage").gameObject;
        _reward = _loadingImage.transform.Find("Reward").gameObject;
    }
}