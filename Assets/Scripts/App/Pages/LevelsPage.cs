using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GrandDevs.AppName
{
    public class LevelsPage : IUIElement
    {
        private GameObject _levelsMenu;
        private IUIManager _uiManager;
        private ILoadObjectsManager _loadObjectsManager;
        private INotificationManager _notificationsManager;
        private ILocalizationManager _localizationManager;
        private IPlayerManager _playerManager;
        private INetworkManager _networkManager;
        private IDataManager _dataManager;
        private ITimerManager _timerManager;
        private IGameManager _gameManager;
        private Button _backButton;
        private List<Button> _levelButtons;

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
            _gameManager = GameClient.Get<IGameManager>();
            _gameManager.LevelCompete += CompleteLevel;
            _levelsMenu = MonoBehaviour.Instantiate(
                _loadObjectsManager.GetObjectByPath<GameObject>("Prefabs/UI/LevelsPage"));
            _levelsMenu.transform.SetParent(_uiManager.Canvas.transform, false);
            _localizationManager.LanguageWasChangedEvent += LanguageWasChangedEventHandler;
            SetLevels();
            _backButton = _levelsMenu.transform.Find("Button_Back").GetComponent<Button>();
            _backButton.onClick.AddListener(OnBackClickHandler);
            _gameManager.LoadCompletedLevelData();
            UpdateLocalization();
            Hide();
        }

        private void SetLevels()
        {
            _levelButtons = new List<Button>();
            for (int i = 0; i <= _gameManager.levelDataJson.Count - 1; i++)
            {
                _levelButtons.Add(_levelsMenu.transform.Find($"Button_Level_{i}").GetComponent<Button>());
            }
            foreach (var levelButton in _levelButtons)
            {
                levelButton.onClick.AddListener(() => OnStartGameClickHandler(_levelButtons.IndexOf(levelButton)));
            }
        }
        private void CompleteLevel(int completeLevel)
        {
            _levelButtons[completeLevel].GetComponent<Image>().color = Color.green;
        }
        public void Update()
        {

        }
        public void Hide()
        {
            _levelsMenu.SetActive(false);
        }

        public void Show()
        {
            _levelsMenu.SetActive(true);
        }

        public void Dispose()
        {

        }
        private void OnStartGameClickHandler(int levelIndex)
        {
            Debug.Log($"Load {levelIndex} level");
            _gameManager.StartGame(levelIndex);
            _uiManager.SetPage<GamePage>();
        }
        private void OnBackClickHandler()
        {
            _uiManager.SetPage<MenuPage>();
        }

        private void LanguageWasChangedEventHandler(Common.Enumerators.Language obj)
        {
            UpdateLocalization();
        }

        private void UpdateLocalization()
        {
            //_loginText.text = _localizationManager.GetUITranslation("KEY_START_SCREEN_LOGIN");
            // }
        }
    }
}

