using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class HealthBar3D : Bar
    {
        public float CurrentHealth => _currentValue;
        public float MaxHealth => _maxValue;

        [SerializeField] private MeshRenderer spriteRenderer;
        [SerializeField] private TextMeshPro levelText;

        private Tween _highlightFillTween;
        private MaterialPropertyBlock _matBlock;
        private float _currentFillAmount;

        private void Awake()
        {
            _matBlock = new MaterialPropertyBlock();
        }

        private void OnDisable()
        {
            _highlightFillTween?.Kill();
        }


        public override void UpdateValue(float currentHealth)
        {
            base.UpdateValue(currentHealth);

            AnimateHpBar();
        }

        public override void Set(float maxHealth, float currentHealth)
        {
            base.Set(maxHealth, currentHealth);
            _highlightFillTween?.Kill();

            _currentFillAmount = Mathf.Clamp01(_currentValue / _maxValue);

            spriteRenderer.GetPropertyBlock(_matBlock);
            _matBlock.SetFloat("_healthNormalized", _currentFillAmount);
            _matBlock.SetFloat("_highlightNormalized", _currentFillAmount);
            spriteRenderer.SetPropertyBlock(_matBlock);
        }

        public override void SetLevel(int level)
        {
            base.SetLevel(level);

            levelText.text = "Lvl " + level.ToString();
        }

        private void AnimateHpBar()
        {
            _highlightFillTween?.Kill();

            float targetFillAmount = Mathf.Clamp01(_currentValue / _maxValue);
            spriteRenderer.GetPropertyBlock(_matBlock);
            _matBlock.SetFloat("_healthNormalized", targetFillAmount);
            spriteRenderer.SetPropertyBlock(_matBlock);

            _highlightFillTween = DOTween.To(() => _currentFillAmount, x => _currentFillAmount = x, targetFillAmount, 1f).OnUpdate(() =>
            {
                spriteRenderer.GetPropertyBlock(_matBlock);
                _matBlock.SetFloat("_highlightNormalized", _currentFillAmount);
                spriteRenderer.SetPropertyBlock(_matBlock);
            });
        }
    }
}