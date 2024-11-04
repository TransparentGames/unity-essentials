using System;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public abstract UIState State { get; }

        public abstract void PrepareOpen();
        public abstract void PrepareClose();


        protected virtual void OnPrepareOpened()
        {

        }

        protected void OnOpened()
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.OpenCallback(State);
        }

        /// <summary>
        /// This is called by UIManager to prepare the view for closing.
        /// </summary>
        protected virtual void OnPrepareClosed()
        {

        }
        protected void OnClosed()
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.CloseCallback(State);
        }
    }
}