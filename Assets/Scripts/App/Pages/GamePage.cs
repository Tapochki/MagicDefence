using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDevs.AppName
{
    public class GamePage : IUIElement
    {

        private GameObject _backGround;
        private IUIManager _uiManager;
        private ILoadObjectsManager _loadObjectsManager;
        private INotificationManager _notificationsManager;
        private ILocalizationManager _localizationManager;
        private IPlayerManager _playerManager;
        private INetworkManager _networkManager;
        private IDataManager _dataManager;
        private ITimerManager _timerManager;
        private IGameManager _gameManager;
        private GameObject _battleground;
        private GameObject _foreground;
        private RawImage _backHouseImage;
        private RawImage _battlegroundImage;
        private Button _pauseButton;
        private Button _waterButton;
        private Button _windButton;
        private Button _groundButton;
        private Button _fireButton;
        private Image _playerHealthBar;
        private Image _playerManaBar;
        private float _scrollingPosition;
        private float _scrollingSpeed;
        private GameObject _userInterface;
        private Text _scoreText;
        private int _maxPlayerHealth;
        private int _maxPlayerMana;
        private Vector2 _scoreTextSize;

        private Transform
            _playerSpawnPosition,
            _upEnemySpawnPosition,
            _leftEnemySpawnPosition,
            _rightEnemySpawnPosition,
            _leftUpEnemySpawnPosition,
            _rightUpEnemySpawnPosition;
        
        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _loadObjectsManager = GameClient.Get<ILoadObjectsManager>();
            _notificationsManager = GameClient.Get<INotificationManager>();
            _localizationManager = GameClient.Get<ILocalizationManager>();
            _playerManager = GameClient.Get<IPlayerManager>();
            _networkManager = GameClient.Get<INetworkManager>();
            _dataManager = GameClient.Get<IDataManager>();
            _timerManager = GameClient.Get<ITimerManager>();
            _backGround = MonoBehaviour.Instantiate(_loadObjectsManager.GetObjectByPath<GameObject>("Prefabs/UI/GameGroundPage"));
            _userInterface = MonoBehaviour.Instantiate(_loadObjectsManager.GetObjectByPath<GameObject>("Prefabs/UI/UserInterface")); 
            _backGround.transform.SetParent(_uiManager.Canvas.transform, false);
            _userInterface.transform.SetParent(_uiManager.InterfaceCanvas.transform, false);
            _localizationManager.LanguageWasChangedEvent += LanguageWasChangedEventHandler;
            _pauseButton = _userInterface.transform.Find("Button_Pause").GetComponent<Button>(); 
            _battleground = _backGround.transform.Find("Battleground").gameObject;
            _waterButton = _userInterface.transform.Find("Button_Water").GetComponent<Button>(); 
            _windButton = _userInterface.transform.Find("Button_Wind").GetComponent<Button>(); 
            _groundButton = _userInterface.transform.Find("Button_Ground").GetComponent<Button>(); 
            _fireButton = _userInterface.transform.Find("Button_Fire").GetComponent<Button>();
            _backHouseImage = _battleground.transform.Find("Image_BackHouse").GetComponent<RawImage>();
            _battlegroundImage =_battleground.transform.Find("Image_Battleground").GetComponent<RawImage>(); 
            _playerHealthBar = _userInterface.transform.Find("Image_HealthBarMask").GetComponent<Image>();
            _playerManaBar = _userInterface.transform.Find("Image_ManaBarMask").GetComponent<Image>();
            _foreground = _battleground.transform.Find("Foreground").gameObject;
            _scoreText = _userInterface.transform.Find("Text_Score").GetComponent<Text>();
            _scoreTextSize = _scoreText.GetComponent<RectTransform>().sizeDelta;
            _gameManager = GameClient.Get<IGameManager>();
            _waterButton.onClick.AddListener(OnWaterButtonClick); 
            _windButton.onClick.AddListener(OnWindButtonClick); 
            _groundButton.onClick.AddListener(OnGroundButtonClick); 
            _fireButton.onClick.AddListener(OnFireButtonClick);
            _gameManager.PlayerController.scoreChange += ScoreTextUpdate;
            _scrollingPosition = 0;
            _scrollingSpeed = 0.0001f;
            _pauseButton.onClick.AddListener(OnPauseClickHandler);
            _gameManager.Battleground = _battleground;
            _gameManager.PlayerController.HealthValueChangeEvent += HealthBarValueChange;
            _gameManager.PlayerController.SetMaxPlayerHealthManaEvent += SetMaxPlayerHealthMana;
            _gameManager.PlayerController.ManaValueChangeEvent += ManaBarValueChange;
            UpdateLocalization();
            Hide();
        }
        public void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow) && _gameManager.PlayerController.mainPlayer.canRightMove)
            {
                _scrollingPosition += _scrollingSpeed * Vector2.right.x;
                _backHouseImage.uvRect = new Rect(_scrollingPosition, _backHouseImage.uvRect.y, _backHouseImage.uvRect.width, _backHouseImage.uvRect.height);
                _battlegroundImage.uvRect = new Rect(_scrollingPosition * 5, _battlegroundImage.uvRect.y, _battlegroundImage.uvRect.width, _battlegroundImage.uvRect.height);
                _foreground.transform.Translate(Vector2.right * -0.3f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && _gameManager.PlayerController.mainPlayer.canLeftMove)
            {
                _scrollingPosition += _scrollingSpeed * Vector2.left.x;
                _backHouseImage.uvRect = new Rect(_scrollingPosition, _backHouseImage.uvRect.y, _backHouseImage.uvRect.width, _backHouseImage.uvRect.height);
                _battlegroundImage.uvRect = new Rect(_scrollingPosition * 5, _battlegroundImage.uvRect.y, _battlegroundImage.uvRect.width, _battlegroundImage.uvRect.height);
                _foreground.transform.Translate(Vector2.left * -0.3f * Time.deltaTime);
            }
        }
        private void SetMaxPlayerHealthMana(int health, int mana)
        {
            _maxPlayerHealth = health;
            _maxPlayerMana = mana;
        }
        public void Hide()
        {
            _backGround.SetActive(false);
            _userInterface.SetActive(false);
        }
        public void Show()
        {
            _backGround.SetActive(true);
            _userInterface.SetActive(true);
        }
        public void Dispose()
        {

        }
        private void ScoreTextUpdate(int scoreNum)
        {
            _scoreText.text = scoreNum.ToString();
        }
        private void OnPauseClickHandler() 
        { 
            _gameManager.PauseGame(); 
            _uiManager.SetPage<PauseMenuPage>(); 
        }
        
        private void HealthBarValueChange(int health) 
        {
            _playerHealthBar.fillAmount = (float)health / _maxPlayerHealth;
        } 
        
        private void ManaBarValueChange(int mana) 
        {
            _playerManaBar.fillAmount = (float)mana / _maxPlayerMana;
        } 
        private void OnWaterButtonClick() 
        { 
            _gameManager.PlayerController.AttackClosetEnemy(Common.Enumerators.ElementType.Water); 
        } 
        private void OnWindButtonClick() 
        { 
            _gameManager.PlayerController.AttackClosetEnemy(Common.Enumerators.ElementType.Wind); 
        } 
        private void OnGroundButtonClick() 
        { 
            _gameManager.PlayerController.AttackClosetEnemy(Common.Enumerators.ElementType.Ground); 
        } 
        private void OnFireButtonClick() 
        { 
            _gameManager.PlayerController.AttackClosetEnemy(Common.Enumerators.ElementType.Fire); 
        } 
        private void LanguageWasChangedEventHandler(Common.Enumerators.Language obj) 
        { 
            UpdateLocalization();
        }
        private void UpdateLocalization()
        {
          //  _loginText.text = _localizationManager.GetUITranslation("KEY_START_SCREEN_LOGIN");
        }
    }
}

