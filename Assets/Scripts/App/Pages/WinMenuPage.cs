using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDevs.AppName
{
    public class WinMenuPage : IUIElement
    { 
        private GameObject _winMenu;
        private IUIManager _uiManager;
        private ILoadObjectsManager _loadObjectsManager;
        private INotificationManager _notificationsManager;
        private ILocalizationManager _localizationManager;
        private IPlayerManager _playerManager;
        private INetworkManager _networkManager;
        private IDataManager _dataManager;
        private ITimerManager _timerManager;
        private IGameManager _gameManager; 
        private Button _nextLevelButton;
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
            _winMenu = MonoBehaviour.Instantiate(
                _loadObjectsManager.GetObjectByPath<GameObject>("Prefabs/UI/WinPage"));
            _winMenu.transform.SetParent(_uiManager.Canvas.transform, false);
            _backToMenu = _winMenu.transform.Find("Button_BackMenu").GetComponent<Button>();
            _nextLevelButton = _winMenu.transform.Find("Button_NextLevel").GetComponent<Button>();
            _backToMenu.onClick.AddListener(OnBackToMenuClickHandler);
            _nextLevelButton.onClick.AddListener(OnNextLevelClickHandler);
            _localizationManager.LanguageWasChangedEvent += LanguageWasChangedEventHandler;
            UpdateLocalization();
            Hide();
        }
        public void Update()
        {

        }
        public void Hide()
        {
            _winMenu.SetActive(false);
        }

        public void Show()
        {
            _winMenu.SetActive(true);
            Debug.Log("Win Menu");
        }

        public void Dispose()
        {

        }
        private void OnNextLevelClickHandler()
        {
            _gameManager.NextLevel();
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

