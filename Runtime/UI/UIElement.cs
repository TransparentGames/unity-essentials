using System;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public event Action TryOpened;
        public event Action TryClosed;

        public abstract UIState State { get; }

        public abstract void TryOpen();
        public abstract void Open();
        public abstract void TryClose();
        public abstract void Close();

        protected void OnTryOpened()
        {
            TryOpened?.Invoke();
        }

        protected void OnTryClosed()
        {
            TryClosed?.Invoke();
        }

        public void OnClosed()
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.CloseCallback(State);
        }
    }
}