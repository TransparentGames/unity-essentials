using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] protected bool showHealthText = false;

        protected float _currentHealth;
        protected float _maxHealth;
        protected float _level;

        public virtual void UpdateHealth(float currentHealth)
        {
            _currentHealth = Mathf.Ceil(currentHealth);
        }

        public virtual void Set(float maxHealth, float currentHealth)
        {
            _maxHealth = Mathf.Ceil(maxHealth);
            _currentHealth = Mathf.Ceil(currentHealth);
        }

        public virtual void SetLevel(int level)
        {
            _level = level;
        }
    }
}