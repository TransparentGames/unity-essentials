using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class HealthBarSpriteRenderer : Bar
    {
        public float CurrentHealth => _currentValue;
        public float MaxHealth => _maxValue;

        [SerializeField] private TextMeshPro levelText;
        [SerializeField] private Transform handleSprite;
        [SerializeField] private Transform highlightSprite;

        private Tween _highlightFillTween;
        private float _currentFillAmount;

        private void LateUpdate()
        {
            AlignCamera();
        }

        private void OnDisable()
        {
            _highlightFillTween?.Kill();
        }

        public override void UpdateValue(float currentHealth)
        {
            base.UpdateValue(currentHealth);

            // Update the handle sprite immediately
            UpdateHandleSprite();

            // Animate the highlight sprite
            AnimateHpBar();
        }

        public override void Set(float maxHealth, float currentHealth)
        {
            base.Set(maxHealth, currentHealth);
            _highlightFillTween?.Kill();

            _currentFillAmount = Mathf.Clamp01(_currentValue / _maxValue);

            // Update the handle sprite immediately
            UpdateHandleSprite();

            // Update the highlight sprite to the initial value
            UpdateHighlightSprite();
        }

        public override void SetLevel(int level)
        {
            base.SetLevel(level);

            levelText.text = level.ToString();
        }

        private void AnimateHpBar()
        {
            _highlightFillTween?.Kill();

            float targetFillAmount = Mathf.Clamp01(_currentValue / _maxValue);

            _highlightFillTween = DOTween.To(() => _currentFillAmount, x => _currentFillAmount = x, targetFillAmount, 1f)
                .OnUpdate(UpdateHighlightSprite);
        }

        private void UpdateHandleSprite()
        {
            float fillAmount = Mathf.Clamp01(_currentValue / _maxValue);
            handleSprite.localScale = new Vector3(fillAmount, handleSprite.localScale.y, handleSprite.localScale.z);
        }

        private void UpdateHighlightSprite()
        {
            highlightSprite.localScale = new Vector3(_currentFillAmount, highlightSprite.localScale.y, highlightSprite.localScale.z);
        }

        private void AlignCamera()
        {
            if (Camera.main != null)
            {
                var camXForm = Camera.main.transform;
                var forward = transform.position - camXForm.position;
                forward.Normalize();
                var up = Vector3.Cross(forward, camXForm.right);
                transform.rotation = Quaternion.LookRotation(forward, up);
            }
        }
    }
}