using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TransparentGames.Essentials.UI
{
    public class View : UIElement
    {
        public event Action TryOpened;
        public event Action TryClosed;
        public event Action Showed;
        public event Action Hided;

        public override UIState State => state;

        [SerializeField] private List<Button> closeButtons;
        [SerializeField] private UIState state;
        [SerializeField] private bool manual;

        public override void TryOpen()
        {
            TryOpened?.Invoke();
            if (manual == false)
                Show();
        }

        public override void TryClose()
        {
            TryClosed?.Invoke();
            if (manual == false)
                Hide();
        }

        public override void Show()
        {
            foreach (var button in closeButtons)
            {
                button.interactable = true;
            }
            gameObject.SetActive(true);
            Showed?.Invoke();
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            Hided?.Invoke();
        }

        private void Awake()
        {
            foreach (var closeButton in closeButtons)
                closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void Start()
        {
            Hide();
        }

        private void OnDestroy()
        {
            foreach (var closeButton in closeButtons)
                closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            foreach (var button in closeButtons)
            {
                button.interactable = false;
            }
            TryClose();
            OnClosed();
        }
    }
}