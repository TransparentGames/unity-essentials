using System;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public abstract UIState State { get; }

        public abstract void PrepareOpen();
        protected abstract void ExecuteOpen();
        public abstract void PrepareClose();
        protected abstract void ExecuteClose();

        protected virtual void OnPrepareOpened()
        {

        }

        protected void OnOpened()
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.OpenCallback(State);
        }

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