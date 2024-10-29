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

        protected virtual void OnPrepareClosed()
        {

        }

        public void OnClosed()
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.CloseCallback(State);
        }
    }
}