using System;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TransparentGames.Essentials.UI
{
    public class HealthBar2D : HealthBar
    {
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;

        [SerializeField] private Image hpBar;
        [SerializeField] private Image animatedHpBar;
        [SerializeField] private TextMeshProUGUI levelText;
        [Space]
        [SerializeField] private TextMeshProUGUI healthText;

        private Tween _highlightTween;

        private void OnDisable()
        {
            _highlightTween?.Kill();
        }

        public override void UpdateHealth(float currentHealth)
        {
            base.UpdateHealth(currentHealth);

            if (showHealthText)
                healthText.text = _currentHealth.ToString() + " / " + _maxHealth.ToString();
            AnimateHpBar();
        }

        public override void Set(float maxHealth, float currentHealth)
        {
            base.Set(maxHealth, currentHealth);
            _highlightTween?.Kill();

            hpBar.fillAmount = Mathf.Clamp01(_currentHealth / _maxHealth);
            animatedHpBar.fillAmount = Mathf.Clamp01(_currentHealth / _maxHealth);
            if (showHealthText)
            {
                healthText.text = _currentHealth.ToString() + " / " + _maxHealth.ToString();
            }
        }

        public override void SetLevel(int level)
        {
            base.SetLevel(level);
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