﻿using System;
using UnityEngine;


namespace GrandDevs.AppName
{
    public interface IMobileKeyboardManager
    {
        event Action KeyboardDoneEvent;
        event Action KeyboardCanceledEvent;
        event Action<string> KeyboardUpdateEvent;

        float CurrentKeyboardHeight { get; }

        void DrawKeyboard(string text, bool hideInput = false, TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default, bool autocorrection = true, bool multiline = false, bool secure = false);
        void HideKeyboard(bool throwDoneEvent, bool throwCanceledEvent);
    }
}