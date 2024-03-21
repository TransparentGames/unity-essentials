using System;

namespace TransparentGames.Essentials.Combat
{
    public interface IHealth
    {
        public float MaxHealth { get; }
        public float CurrentHealth { get; }

        public event Action Enabled;
        public event Action<float, float> ValueChanged;
        public event Action ValueZeroed;

        public void Add(float amount);
    }
}