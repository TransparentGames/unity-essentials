using System;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TransparentGames.UI
{
    public class HealthBar : MonoBehaviour
    {
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;

        [SerializeField] private Image hpBar;
        [SerializeField] private Image animatedHpBar;
        [SerializeField] private TextMeshProUGUI levelText;
        [Space]
        [SerializeField] private bool showHealthText = false;
        [SerializeField] private TextMeshProUGUI currentHealthText;
        [SerializeField] private TextMeshProUGUI maxHealthText;

        private Tween _highlightTween;
        private float _currentHealth;
        private float _maxHealth;

        private void OnDisable()
        {
            _highlightTween?.Kill();
        }

        public void UpdateHealth(float currentHealth)
        {
            _currentHealth = currentHealth;
            if (showHealthText)
                currentHealthText.text = currentHealth.ToString();
            AnimateHpBar();
        }

        public void Set(float maxHealth, float currentHealth)
        {
            _highlightTween?.Kill();
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;
            hpBar.fillAmount = Mathf.Clamp01(_currentHealth / _maxHealth);
            animatedHpBar.fillAmount = Mathf.Clamp01(_currentHealth / _maxHealth);
            if (showHealthText)
            {
                maxHealthText.text = maxHealth.ToString();
                currentHealthText.text = currentHealth.ToString();
            }
        }

        public void SetLevel(int level)
        {
            levelText.text = "Lvl " + level.ToString();
        }

        private void AnimateHpBar()
        {
            float fillAmount = Mathf.Clamp01(_currentHealth / _maxHealth);
            hpBar.fillAmount = fillAmount;

            _highlightTween?.Kill();
            _highlightTween = animatedHpBar.DOFillAmount(fillAmount, 1f);
        }
    }
}