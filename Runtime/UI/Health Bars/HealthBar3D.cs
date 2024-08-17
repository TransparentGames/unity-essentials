using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class HealthBar3D : HealthBar
    {
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;

        [SerializeField] private MeshRenderer spriteRenderer;
        [SerializeField] private TextMeshPro levelText;
        [Space]
        [SerializeField] private TextMeshPro currentHealthText;
        [SerializeField] private TextMeshPro maxHealthText;

        private Tween _highlightFillTween;
        private MaterialPropertyBlock _matBlock;
        private float _currentFillAmount;

        private void Awake()
        {
            _matBlock = new MaterialPropertyBlock();
        }

        private void LateUpdate()
        {
            AlignCamera();
        }

        private void OnDisable()
        {
            _highlightFillTween?.Kill();
        }


        public override void UpdateHealth(float currentHealth)
        {
            base.UpdateHealth(currentHealth);

            if (showHealthText)
                currentHealthText.text = currentHealth.ToString();
            AnimateHpBar();
        }

        public override void Set(float maxHealth, float currentHealth)
        {
            base.Set(maxHealth, currentHealth);
            _highlightFillTween?.Kill();

            _currentFillAmount = Mathf.Clamp01(_currentHealth / _maxHealth);

            spriteRenderer.GetPropertyBlock(_matBlock);
            _matBlock.SetFloat("_healthNormalized", _currentFillAmount);
            _matBlock.SetFloat("_highlightNormalized", _currentFillAmount);
            spriteRenderer.SetPropertyBlock(_matBlock);

            if (showHealthText)
            {
                maxHealthText.text = maxHealth.ToString();
                currentHealthText.text = currentHealth.ToString();
            }
        }

        public override void SetLevel(int level)
        {
            base.SetLevel(level);

            levelText.text = "Lvl " + level.ToString();
        }

        private void AnimateHpBar()
        {
            _highlightFillTween?.Kill();

            float targetFillAmount = Mathf.Clamp01(_currentHealth / _maxHealth);
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