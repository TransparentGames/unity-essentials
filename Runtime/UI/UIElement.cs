using System;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public event Action Closed;

        public abstract UIState State { get; }

        public abstract void TryOpen();
        public abstract void Open();
        public abstract void TryClose();
        public abstract void Close();

        protected void OnClosed()
        {
            Closed?.Invoke();
        }
    }
}