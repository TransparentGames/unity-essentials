using System;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    [CreateAssetMenu(fileName = "UI State", menuName = "Transparent Games/UI/UI State", order = 0)]
    public class UIState : ScriptableObject
    {
        public void TryOpen()
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.TryOpen(this);
        }

        public void TryClose()
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.ForceClose(this);
        }

        public static bool operator ==(UIState a, UIState b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return a.name == b.name;
        }

        public static bool operator !=(UIState a, UIState b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is UIState other)
            {
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return name != null ? name.GetHashCode() : 0;
        }
    }
}