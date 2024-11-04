using System;
using TransparentGames.Essentials.UI;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class Window : UIElement
    {
        public override UIState State => state;

        [SerializeField] private UIState state;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public override void PrepareOpen()
        {
            gameObject.SetActive(true);
        }

        public override void PrepareClose()
        {
            gameObject.SetActive(false);
        }
    }
}