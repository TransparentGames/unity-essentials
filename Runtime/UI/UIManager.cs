using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Dictionary<UIState, UIElement> _UIElements = new();

        protected override void Awake()
        {
            var uiElements = GetComponentsInChildren<UIElement>(true);

            foreach (var uiElement in uiElements)
            {
                if (uiElement.State)
                    _UIElements.Add(uiElement.State, uiElement);
            }

            base.Awake();
        }

        public void Open(UIState state)
        {
            if (_UIElements.TryGetValue(state, out var uiElement))
            {
                uiElement.TryOpen();
            }
        }

        public void Close(UIState state)
        {
            if (_UIElements.TryGetValue(state, out var uiElement))
            {
                uiElement.TryClose();
            }
        }
    }
}