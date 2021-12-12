using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDevs.AppName
{
    public class PauseMenuPage : IUIElement
    { 
        private GameObject _pauseMenu;
        private IUIManager _uiManager;
        private ILoadObjectsManager _loadObjectsManager;
        private INotificationManager _notificationsManager;
        private ILocalizationManager _localizationManager;
        private IPlayerManager _playerManager;
        private INetworkManager _networkManager;
        private IDataManager _dataManager;
        private ITimerManager _timerManager;
        private IGameManager _gameManager;
        private Button _reStartButton;
        private Button _continueButton;
        private Button _backToMenu;
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
            _pauseMenu = MonoBehaviour.Instantiate(
                _loadObjectsManager.GetObjectByPath<GameObject>("Prefabs/UI/PausePage"));
            _pauseMenu.transform.SetParent(_uiManager.Canvas.transform, false);
            _localizationManager.LanguageWasChangedEvent += LanguageWasChangedEventHandler;
            _backToMenu = _pauseMenu.transform.Find("Button_BackMenu").GetComponent<Button>();
            _reStartButton = _pauseMenu.transform.Find("Button_Restart").GetComponent<Button>();
            _continueButton = _pauseMenu.transform.Find("Button_Continue").GetComponent<Button>();
            _reStartButton.onClick.AddListener(OnRestartCLickGameHandler);
            _backToMenu.onClick.AddListener(OnBackToMenuClickHandler);
            _continueButton.onClick.AddListener(OnContinueClickHandler);
            UpdateLocalization();
            Hide();
        }
        public void Update()
        {

        }
        public void Hide()
        {
            _pauseMenu.SetActive(false);
        }

        public void Show()
        {
            _pauseMenu.SetActive(true);
        }

        public void Dispose()
        {

        }
        private void OnContinueClickHandler()
        {
            _gameManager.ContinueGame();
            _uiManager.SetPage<GamePage>();
        }
        private void OnRestartCLickGameHandler()
        {
            _gameManager.RestartGame();
            _uiManager.SetPage<GamePage>();
        }
        private void OnBackToMenuClickHandler()
        {
            _gameManager.ClearData();
            _uiManager.SetPage<MenuPage>();
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
