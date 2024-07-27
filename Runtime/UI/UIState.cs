using System;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    [CreateAssetMenu(fileName = "UI State", menuName = "Transparent Games/UI/UI State", order = 0)]
    public class UIState : ScriptableObject
    {
        public void TryOpen()
        {
            UIManager.Initialized(() => UIManager.Instance.TryOpen(this));
        }

        public void TryClose()
        {
            UIManager.Initialized(() => UIManager.Instance.TryClose(this));
        }
    }
}