using System;
using System.Collections.Generic;
using UnityEngine;


namespace GrandDevs.AppName
{
    public class InputManager : IService, IInputManager
    {
        private List<InputEvent> _inputHandlers;

        private int _customFreeIndex = 0;


        public void Init()
        {
            _inputHandlers = new List<InputEvent>();
        }

        public void Update()
        {
            HandleKeyboardInput();
        }

        public void Dispose()
        {
            _inputHandlers.Clear();
        }

        /// <summary>
        /// return an index of created event handler
        /// </summary>
        /// <param name="code"></param>
        /// <param name="onInputUp"></param>
        /// <param name="onInputDown"></param>
        /// <param name="onInput"></param>
        /// <returns></returns>
        public int RegisterKeyboardInputHandler(KeyCode code, Action onInputUp = null, Action onInputDown = null, Action onInput = null)
        {
            var item = new InputKeyboardEvent()
            {
                keyCode = code,
                OnInputEvent = onInput,
                OnInputDownEvent = onInputDown,
                OnInputUpEvent = onInputUp,
            };

            item.index = _customFreeIndex++;

            _inputHandlers.Add(item);

            return item.index;
        }

        public void UnregisterKeyboardInputHandler(int index)
        {
            var inputHandler = _inputHandlers.Find(x => x.index == index);

            if (inputHandler != null)
                _inputHandlers.Remove(inputHandler);
        }

        private void HandleKeyboardInput()
        {
            if (_inputHandlers.Count > 0)
            {
                InputKeyboardEvent keyboardEvent;
                foreach (var item in _inputHandlers)
                {
                    if (item is InputKeyboardEvent)
                    {
                        keyboardEvent = (InputKeyboardEvent)item;

                        if (Input.GetKey(keyboardEvent.keyCode))
                            keyboardEvent.ThrowOnInputEvent();

                        if (Input.GetKeyUp(keyboardEvent.keyCode))
                            keyboardEvent.ThrowOnInputUpEvent();

                        if (Input.GetKeyDown(keyboardEvent.keyCode))
                            keyboardEvent.ThrowOnInputDownEvent();
                    }
                }
            }
        }
    }

    public class InputEvent
    {
        public int index;

        public Action OnInputUpEvent;
        public Action OnInputDownEvent;
        public Action OnInputEvent;

        public void ThrowOnInputUpEvent()
        {
            if (OnInputUpEvent != null)
                OnInputUpEvent();
        }

        public void ThrowOnInputDownEvent()
        {
            if (OnInputDownEvent != null)
                OnInputDownEvent();
        }

        public void ThrowOnInputEvent()
        {
            if (OnInputEvent != null)
                OnInputEvent();
        }
    }

    public class InputKeyboardEvent : InputEvent
    {
        public KeyCode keyCode;
    }

    public class InputMouseEvent : InputEvent
    {
        public int mouseCode;
    }
}