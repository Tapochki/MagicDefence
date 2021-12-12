using GrandDevs.AppName.Common;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDevs.AppName
{
    public class MenuPage : IUIElement
    {
        private GameObject _mainMenu;
        private IUIManager _uiManager;
        private ILoadObjectsManager _loadObjectsManager;
        private INotificationManager _notificationsManager;
        private ILocalizationManager _localizationManager;
        private IPlayerManager _playerManager;
        private INetworkManager _networkManager;
        private IDataManager _dataManager;
        private ITimerManager _timerManager;
        private IGameManager _gameManager;
        private Button _startButton;
        private Button _exitButton;

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
            _mainMenu = MonoBehaviour.Instantiate(
                _loadObjectsManager.GetObjectByPath<GameObject>("Prefabs/UI/MenuPage"));
            _mainMenu.transform.SetParent(_uiManager.Canvas.transform, false);
            _localizationManager.LanguageWasChangedEvent += LanguageWasChangedEventHandler;
            _startButton = _mainMenu.transform.Find("Button_Start").GetComponent<Button>();
            _exitButton = _mainMenu.transform.Find("Button_Exit").GetComponent<Button>();
            _startButton.onClick.AddListener(OnStartGameClickHandler);
            _exitButton.onClick.AddListener(OnExitGameClickHandler);
            UpdateLocalization();
            Hide();
        }

        public void Update()
        {

        }


        public void Hide()
        {
            _mainMenu.SetActive(false);
        }

        public void Show()
        {
            _mainMenu.SetActive(true);
        }

        public void Dispose()
        {

        }
        private void OnStartGameClickHandler()
        {
            _uiManager.SetPage<LevelsPage>();
        }
        private void OnExitGameClickHandler()
        {
            Application.Quit();
        }

        private void LanguageWasChangedEventHandler(Enumerators.Language obj)
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