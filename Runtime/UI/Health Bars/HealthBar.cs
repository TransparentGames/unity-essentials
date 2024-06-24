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
            AnimateHpBar();
        }

        public void Set(float maxHealth, float currentHealth)
        {
            _highlightTween?.Kill();
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;
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