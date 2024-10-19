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

        private Dictionary<UIState, UIElement> _UIElements = new();

        protected override void Awake()
        {
            var uiElements = GetComponentsInChildren<UIElement>(true);

            foreach (var uiElement in uiElements)
            {
                if (uiElement.State)
                {
                    _UIElements.Add(uiElement.State, uiElement);
                }
            }

            base.Awake();
        }

        public void TryOpen(UIState state)
        {
            if (_UIElements.TryGetValue(state, out var uiElement))
            {
                UIStateChangeAttempt?.Invoke(state);
            }
        }

        public void ForceOpen(UIState state)
        {
            if (_UIElements.TryGetValue(state, out var uiElement))
            {
                uiElement.TryOpen();
            }
        }

        public UIElement Get(UIState state)
        {
            if (_UIElements.TryGetValue(state, out var uiElement))
            {
                return uiElement;
            }

            return null;
        }

        public void ForceClose(UIState state)
        {
            if (_UIElements.TryGetValue(state, out var uiElement))
            {
                uiElement.TryClose();
            }
        }

        public void CloseCallback(UIState state)
        {
            if (_UIElements.TryGetValue(state, out var uiElement))
            {
                UiStateClosed.Invoke(state);
            }
        }
    }
}