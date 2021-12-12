using FrostweepGames.Internal;
using GrandDevs.AppName.Common;
using System;
using UnityEngine;


namespace GrandDevs.AppName
{
    public sealed class AppStateManager : IService, IAppStateManager
    {
        private IUIManager _uiManager;
        private IDataManager _dataManager;
        private IPlayerManager _playerManager;
        private ILocalizationManager _localizationManager;
        private INotificationManager _notificationsManager;

        private float _backButtonTimer,
                      _backButtonResetDelay = 0.5f;

        private int _backButtonClicksCount;
        private bool _isBackButtonCounting;

        public Enumerators.AppState AppState { get; set; }

        public void Dispose()
        {

        }

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _dataManager = GameClient.Get<IDataManager>();
            _playerManager = GameClient.Get<IPlayerManager>();
            _localizationManager = GameClient.Get<ILocalizationManager>();
            _notificationsManager = GameClient.Get<INotificationManager>();
        }

        public void Update()
        {
            CheckBackButton();
        }

        public void ChangeAppState(Enumerators.AppState stateTo)
        {
            if (AppState == stateTo)
                return;

            switch (stateTo)
            {
                case Enumerators.AppState.APP_INIT_LOADING:
                    {
                        //_uiManager.SetPage<LoadingAppPage>();
                        _dataManager.StartLoadCache();
                    }
                    break;
                case Enumerators.AppState.MAIN_MENU:
                    {
                        _uiManager.SetPage<MenuPage>();
                    }
                    break;
                default:
                    throw new NotImplementedException("Not Implemented " + stateTo.ToString() + " state!");
            }

            AppState = stateTo;
        }

        public void PauseGame(bool enablePause)
        {
            if (enablePause)
            {
                Utilites.DebugLog("App is paused");

                Time.timeScale = 0;
            }
            else
            {
                Utilites.DebugLog("App is unpaused");

                Time.timeScale = 1;
            }
        }

        private void CheckBackButton()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isBackButtonCounting = true;
                _backButtonClicksCount++;
                _backButtonTimer = 0f;

                if (_backButtonClicksCount >= 2)
                {
                    Application.Quit();
                }
            }

            if (_isBackButtonCounting)
            {
                _backButtonTimer += Time.deltaTime;

                if (_backButtonTimer >= _backButtonResetDelay)
                {
                    _backButtonTimer = 0f;
                    _backButtonClicksCount = 0;
                    _isBackButtonCounting = false;
                }
            }
        }
    }
}