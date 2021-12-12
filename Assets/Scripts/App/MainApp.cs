﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GrandDevs.AppName
{
    public class MainApp : MonoBehaviour
    {
        public delegate void MainAppDelegate(object param);
        public event MainAppDelegate OnLevelWasLoadedEvent;

        public event Action LateUpdateEvent;
        public event Action FixedUpdateEvent;

        private static MainApp _Instance;
        public static MainApp Instance
        {
            get { return _Instance; }
            private set { _Instance = value; }
        }

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (Instance == this)
            {
                GameClient.Instance.InitServices();
                GameClient.Get<IAppStateManager>().ChangeAppState(Common.Enumerators.AppState.APP_INIT_LOADING);
                SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            }
        }

        private void Update()
        {
            if (Instance == this)
                GameClient.Instance.Update();
        }

        private void LateUpdate()
        {
            if (Instance == this)
            {
                if (LateUpdateEvent != null)
                    LateUpdateEvent();
            }
        }

        private void FixedUpdate()
        {
            if (Instance == this)
            {
                if (FixedUpdateEvent != null)
                    FixedUpdateEvent();
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                GameClient.Instance.Dispose();
        }


        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (Instance == this)
            {
                if (OnLevelWasLoadedEvent != null)
                    OnLevelWasLoadedEvent(arg0.buildIndex);
            }
        }
    }
}