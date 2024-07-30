using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TransparentGames.Essentials.UI
{
    public class View : UIElement
    {
        public override UIState State => state;

        [SerializeField] private UIState state;
        [SerializeField] private bool manual;

        public override void TryOpen()
        {
            OnTryOpened();
            if (manual == false)
                Open();
        }

        public override void TryClose()
        {
            OnTryClosed();
            if (manual == false)
                Close();
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            OnOpened();
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            OnClosed();
        }

        private void Start()
        {
            Close();
        }
    }
}