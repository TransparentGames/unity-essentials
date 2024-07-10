using System;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    [CreateAssetMenu(fileName = "UI State", menuName = "Transparent Games/UI/UI State", order = 0)]
    public class UIState : ScriptableObject
    {
        public void Open()
        {
            UIManager.Initialized(() => UIManager.Instance.Open(this));
        }

        public void Close()
        {
            UIManager.Initialized(() => UIManager.Instance.Close(this));
        }

        public void AddListener(Action callback)
        {
            UIManager.Initialized(() => UIManager.Instance.Get(this).Closed += callback);
        }

        public void RemoveListener(Action callback)
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.Get(this).Closed -= callback;
        }
    }
}