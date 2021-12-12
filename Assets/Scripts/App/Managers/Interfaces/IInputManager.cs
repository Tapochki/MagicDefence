using System;
using UnityEngine;

namespace GrandDevs.AppName
{
    public interface IInputManager
    {
        int RegisterKeyboardInputHandler(KeyCode code, Action onInputUp = null, Action onInputDown = null, Action onInput = null);
        void UnregisterKeyboardInputHandler(int index);
    }
}
