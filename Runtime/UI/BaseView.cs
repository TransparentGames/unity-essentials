using System;
using TransparentGames.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TransparentGames.UI
{
    public class BaseView : View
    {
        public event Action Showed;
        public event Action Hided;

        [SerializeField] private Button closeButton;

        public override void Show()
        {
            closeButton.interactable = true;
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
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void Start()
        {
            Hide();
        }


        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            closeButton.interactable = false;
            Hide();
            OnClosed();
        }
    }
}