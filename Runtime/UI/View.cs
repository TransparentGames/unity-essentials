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
        public event Action Opened;

        public override UIState State => state;

        [SerializeField] private List<Button> closeButtons;
        [SerializeField] private UIState state;
        [SerializeField] private bool manual;

        public override void TryOpen()
        {
            TryOpened?.Invoke();
            if (manual == false)
                Open();
        }

        public override void TryClose()
        {
            TryClosed?.Invoke();
            if (manual == false)
                Close();
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            Opened?.Invoke();
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            OnClosed();
        }

        private void Awake()
        {
            foreach (var closeButton in closeButtons)
                closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void Start()
        {
            Close();
        }

        private void OnDestroy()
        {
            foreach (var closeButton in closeButtons)
                closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            TryClose();
        }
    }
}