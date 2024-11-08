using System;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TransparentGames.Essentials.UI
{
    public class HealthBar2D : Bar
    {
        public float CurrentHealth => _currentValue;
        public float MaxHealth => _maxValue;

        [SerializeField] private Image hpBar;
        [SerializeField] private Image animatedHpBar;
        [SerializeField] private TextMeshProUGUI levelText;
        [Space]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] protected bool showHealthText = false;

        private Tween _highlightTween;

        private void OnDisable()
        {
            _highlightTween?.Kill();
        }

        public override void UpdateValue(float currentHealth)
        {
            base.UpdateValue(currentHealth);

            if (showHealthText)
                healthText.text = _currentValue.ToString() + " / " + _maxValue.ToString();
            AnimateHpBar();
        }

        public override void Set(float maxHealth, float currentHealth)
        {
            base.Set(maxHealth, currentHealth);
            _highlightTween?.Kill();

            hpBar.fillAmount = Mathf.Clamp01(_currentValue / _maxValue);
            animatedHpBar.fillAmount = Mathf.Clamp01(_currentValue / _maxValue);
            if (showHealthText)
            {
                healthText.text = _currentValue.ToString() + " / " + _maxValue.ToString();
            }
        }

        public override void SetLevel(int level)
        {
            base.SetLevel(level);
            levelText.text = "Lvl " + level.ToString();
        }

        private void AnimateHpBar()
        {
            float fillAmount = Mathf.Clamp01(_currentValue / _maxValue);
            hpBar.fillAmount = fillAmount;

            _highlightTween?.Kill();
            _highlightTween = animatedHpBar.DOFillAmount(fillAmount, 1f);
        }
    }
}