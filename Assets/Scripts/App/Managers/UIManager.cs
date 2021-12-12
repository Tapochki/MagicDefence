﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDevs.AppName
{
    public class UIManager : IService, IUIManager
    {
        public List<IUIElement> Pages
        {
            get { return _uiPages; }
        }

        private List<IUIElement> _uiPages;
        private List<IUIPopup> _uiPopups;

        public IUIElement CurrentPage { get; set; }

        public CanvasScaler CanvasScaler { get; set; }
        public GameObject Canvas { get; set; }
        public GameObject InterfaceCanvas { get; set; }
        public GameManager GameManager;

        public void Dispose()
        {
            foreach (var page in _uiPages)
                page.Dispose();

            foreach (var popup in _uiPopups)
                popup.Dispose();
        }

        public void Init()
        {
            Canvas = GameObject.Find("Canvas");
            InterfaceCanvas = GameObject.Find("InterfaceCanvas");
            CanvasScaler = Canvas.GetComponent<CanvasScaler>();
            _uiPages = new List<IUIElement>();
            _uiPages.Add(new MenuPage());
            _uiPages.Add(new LevelsPage());
            _uiPages.Add(new PauseMenuPage());
            _uiPages.Add(new WinMenuPage());
            _uiPages.Add(new LoseMenuPage());
            _uiPages.Add(new GamePage());

            foreach (var page in _uiPages)
                page.Init();

            _uiPopups = new List<IUIPopup>();
            //_uiPopups.Add(new NotificationsPopup());

            foreach (var popup in _uiPopups)
                popup.Init();
        }

        public void Update()
        {
            foreach (var page in _uiPages)
                page.Update();

            foreach (var popup in _uiPopups)
                popup.Update();
        }

        public void HideAllPages()
        {
            foreach (var _page in _uiPages)
            {
                _page.Hide();
            }
        }

        public void SetPage<T>(bool hideAll = false) where T : IUIElement
        {
            if (hideAll)
            {
                HideAllPages();
            }
            else
            {
                if (CurrentPage != null)
                    CurrentPage.Hide();
            }

            foreach (var _page in _uiPages)
            {
                if (_page is T)
                {
                    CurrentPage = _page;
                    break;
                }
            }

            CurrentPage.Show();
        }

        public void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }

            if (setMainPriority)
                popup.SetMainPriority();

            if (message == null)
                popup.Show();
            else
                popup.Show(message);
        }

        public void HidePopup<T>() where T : IUIPopup
        {
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    _popup.Hide();
                    break;
                }
            }
        }

        public IUIPopup GetPopup<T>() where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }

            return popup;
        }

        public IUIElement GetPage<T>() where T : IUIElement
        {
            IUIElement page = null;
            foreach (var _page in _uiPages)
            {
                if (_page is T)
                {
                    page = _page;
                    break;
                }
            }
            return page;
        }
    }
}