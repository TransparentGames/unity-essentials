using System;
using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public enum UIRequest
    {
        Open,
        Close,
        Cancel
    }

    public class UIManager : MonoSingleton<UIManager>
    {
        public UIState[] History => _uiStateHistory.ToArray();
        public event Action<UIState, UIRequest> UIStateRequestAttempt;
        public event Action<UIState> UIStateClosed;

        private Dictionary<string, UIElement> _UIElements = new();
        private Stack<UIState> _uiStateHistory = new();

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
            if (_UIElements.TryGetValue(state.name, out var uiElement) == false)
                return;

            if (_uiStateHistory.Count > 0 && _uiStateHistory.Peek().name == state.name)
                return;

            UIStateRequestAttempt?.Invoke(state, UIRequest.Open);
        }

        /// <summary>
        /// This is called by Managers to force open a view.
        /// </summary>
        /// <param name="state"></param>
        public void ForceOpen(UIState state)
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement) == false)
                return;

            uiElement.PrepareOpen();
        }

        public void OpenCallback(UIState state)
        {
            _uiStateHistory.Push(state);
        }

        public UIElement Get(UIState state)
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement))
            {
                return uiElement;
            }

            return null;
        }

        public T Get<T>(UIState state) where T : UIElement
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement))
            {
                return uiElement as T;
            }

            return null;
        }

        public void TryCancel(UIState state)
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement) == false)
                return;

            if (_uiStateHistory.Count == 0)
                return;

            if (_uiStateHistory.Peek().name != state.name)
            {
                Debug.LogWarning($"Trying to close {state.name} but the top of the stack is {_uiStateHistory.Peek().name}");
                return;
            }

            if (state.canBeForceClosed == false)
            {
                UIStateRequestAttempt?.Invoke(state, UIRequest.Cancel);
            }
            else
            {
                ForceClose(state);
            }
        }

        public void TryClose(UIState state)
        {
            if (_UIElements.TryGetValue(state.name, out var uiElement) == false)
                return;

            if (_uiStateHistory.Count == 0)
                return;

            if (_uiStateHistory.Contains(state) == false)
                return;

            UIStateRequestAttempt?.Invoke(state, UIRequest.Close);
        }

        public void TryClose(UIElement element)
        {
            if (element == null)
                return;

            TryClose(element.State);
        }

        /// <summary>
        /// This is called by Managers to force close a view.
        /// </summary>
        /// <param name="state"></param>
        public void ForceClose(UIState state)
        {
            if (_uiStateHistory.Count == 0)
                return;

            if (_UIElements.TryGetValue(state.name, out var uiElement) == false)
                return;

            if (_uiStateHistory.Contains(state) == false)
                return;

            // Create a list to hold the states to be closed
            var statesToClose = new List<UIState>();

            // Iterate through the history stack to find the specified state
            foreach (var currentState in _uiStateHistory)
            {
                statesToClose.Add(currentState);

                if (currentState.name == state.name)
                    break;
            }

            // Close all the states in the list
            foreach (var stateToClose in statesToClose)
            {
                if (_UIElements.TryGetValue(stateToClose.name, out var elementToClose))
                {
                    elementToClose.PrepareClose();
                }
            }
        }

        /// <summary>
        /// This is called by UIElement when it is closing, to notify all subscribers about view being closed.
        /// </summary>
        /// <param name="state"></param>
        public void CloseCallback(UIState state)
        {
            if (_uiStateHistory.Count > 0 && _uiStateHistory.Peek().name == state.name)
            {
                var stackState = _uiStateHistory.Pop();
                UIStateClosed?.Invoke(stackState);
            }
        }
    }
}