using System;
using System.Collections.Generic;
using TransparentGames.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TransparentGames.UI
{
    public class BaseView : View
    {
        public event Action Showed;
        public event Action Hided;

        [SerializeField] private List<Button> closeButtons;

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
            Hide();
            OnClosed();
        }
    }
}