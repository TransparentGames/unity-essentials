using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class Bar : MonoBehaviour
    {
        protected float _currentValue;
        protected float _maxValue;
        protected float _level;

        public virtual void UpdateValue(float currentValue)
        {
            _currentValue = Mathf.Ceil(currentValue);
        }

        public virtual void Set(float maxValue, float currentValue)
        {
            _maxValue = Mathf.Ceil(maxValue);
            _currentValue = Mathf.Ceil(currentValue);
        }

        public virtual void SetLevel(int level)
        {
            _level = level;
        }
    }
}