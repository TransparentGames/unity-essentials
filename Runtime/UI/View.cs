using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TransparentGames.Essentials.UI
{
    public class View : UIElement
    {
        public event Action Opened;
        public override UIState State => state;

        [SerializeField] private UIState state;

        protected virtual void Start()
        {
            gameObject.SetActive(false);
        }

        public override void PrepareOpen()
        {
            OnPrepareOpened();
            gameObject.SetActive(true);
            OnOpened();
        }

        protected void Close()
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.TryClose(State);
        }

        public override void PrepareClose()
        {
            OnPrepareClosed();
            gameObject.SetActive(false);
            OnClosed();
        }
    }
}