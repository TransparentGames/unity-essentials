using System;
using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public event Action<UIState> UIStateChangeAttempt;
        public event Action<UIState> UiStateClosed;

        private Dictionary<string, UIElement> _UIElements = new();

        protected override void Awake()
        {
            var uiElements = GetComponentsInChildren<UIElement>(true);

            foreach (var uiElement in uiElements)
            {
                if (uiElement.State)
                {
                    _UIElements.Add(uiElement.State.name, uiElement);
                }
            }

            base.Awake();
        }

        public void TryOpen(UIState state)
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement))
            {
                UIStateChangeAttempt?.Invoke(state);
            }
        }

        /// <summary>
        /// This is called by Managers to force open a view.
        /// </summary>
        /// <param name="state"></param>
        public void ForceOpen(UIState state)
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement))
            {
                uiElement.PrepareOpen();
            }
        }

        public UIElement Get(UIState state)
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement))
            {
                return uiElement;
            }

            return null;
        }

        /// <summary>
        /// This is called by Managers to force close a view.
        /// </summary>
        /// <param name="state"></param>
        public void TryClose(UIState state)
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement))
            {
                uiElement.PrepareClose();
            }
        }

        /// <summary>
        /// This is called by UIElement when it is closing, to notify all subscribers about view being closed.
        /// </summary>
        /// <param name="state"></param>
        public void CloseCallback(UIState state)
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement))
            {
                UiStateClosed.Invoke(state);
            }
        }
    }
}