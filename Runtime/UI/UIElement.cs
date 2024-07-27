using System;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public event Action TryOpened;
        public event Action Opened;
        public event Action TryClosed;
        public event Action Closed;

        public abstract UIState State { get; }

        public abstract void TryOpen();
        public abstract void Open();
        public abstract void TryClose();
        public abstract void Close();

        protected void OnTryOpened()
        {
            TryOpened?.Invoke();
        }

        protected void OnOpened()
        {
            Opened?.Invoke();
        }

        protected void OnTryClosed()
        {
            TryClosed?.Invoke();
        }

        protected void OnClosed()
        {
            Closed?.Invoke();
        }
    }
}