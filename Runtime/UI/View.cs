using System;
using UnityEngine;

namespace TransparentGames.UI
{
    public abstract class View : MonoBehaviour
    {
        public event Action Closed;
        public abstract void Show();
        public abstract void Hide();

        protected void OnClosed()
        {
            Closed?.Invoke();
        }
    }
}