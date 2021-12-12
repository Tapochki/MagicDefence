using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDevs.AppName
{
    public interface IUIManager
    {
        GameObject Canvas { get; set; }
        public GameObject InterfaceCanvas { get; set; }
        CanvasScaler CanvasScaler { get; set; }

        IUIElement CurrentPage { get; set; }
        void SetPage<T>(bool hideAll = false) where T : IUIElement;
        void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup;
        void HidePopup<T>() where T : IUIPopup;
        IUIPopup GetPopup<T>() where T : IUIPopup;
        IUIElement GetPage<T>() where T : IUIElement;

        void HideAllPages();
    }
}