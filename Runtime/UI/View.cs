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

        protected virtual void Start()
        {
            gameObject.SetActive(false);
        }

        public override void PrepareOpen()
        {
            OnPrepareOpened();
            if (manual == false)
                ExecuteOpen();
        }

        protected override void ExecuteOpen()
        {
            gameObject.SetActive(true);
        }

        public override void PrepareClose()
        {
            OnPrepareClosed();
            if (manual == false)
                ExecuteClose();
        }

        protected override void ExecuteClose()
        {
            gameObject.SetActive(false);
            OnClosed();
        }
    }
}