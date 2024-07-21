using System;
using TransparentGames.Essentials.UI;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class Window : UIElement
    {
        public override UIState State => _state;

        [SerializeField] private UIState _state;


        public override void Close()
        {
            gameObject.SetActive(false);
            OnClosed();
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            OnOpened();
        }

        public override void TryClose()
        {
            Close();
        }

        public override void TryOpen()
        {
            Open();
        }
    }
}