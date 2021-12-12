using UnityEngine;

namespace GrandDevs.AppName
{
    public interface IUIElement
    {
        void Init();
        void Show();
        void Hide();
        void Update();
        void Dispose();
    }
}