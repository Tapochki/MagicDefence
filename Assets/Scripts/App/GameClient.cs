﻿namespace GrandDevs.AppName
{
    public class GameClient : ServiceLocatorBase
    {
        private static object _sync = new object();

        private static GameClient _Instance;
        
        public static GameClient Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_sync)
                    {
                        _Instance = new GameClient();
                    }
                }
                return _Instance;
            }
        }

        public static bool IsDebugMode = false; //change to 'false' for release

        /// <summary>
        /// Initializes a new instance of the <see cref="GameClient"/> class.
        /// </summary>
        internal GameClient()
            : base()
        {
#if UNITY_EDITOR
            IsDebugMode = false;
#endif
            AddService<IGameManager>(new GameManager());
            AddService<ITimerManager>(new TimerManager());
            AddService<IInputManager>(new InputManager());
            AddService<IMobileKeyboardManager>(new MobileKeyboardManager());
            AddService<ILocalizationManager>(new LocalizationManager());
            AddService<INetworkManager>(new NetworkManager());
            AddService<IPlayerManager>(new PlayerManager());
            AddService<ILoadObjectsManager>(new LoadObjectsManager());
            AddService<IAppStateManager>(new AppStateManager());
            AddService<ISoundManager>(new SoundManager());
            AddService<INotificationManager>(new NotificationManager());
            AddService<IUIManager>(new UIManager());
            AddService<IScenesManager>(new ScenesManager());
            AddService<IDataManager>(new DataManager());
            AddService<IScreenOrientationManager>(new ScreenOrientationManager());
            AddService<INavigationManager>(new NavigationManager());
        }

        public static T Get<T>()
        {
            return Instance.GetService<T>();
        }
    }
}