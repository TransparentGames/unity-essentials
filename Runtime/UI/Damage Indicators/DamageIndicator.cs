using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TransparentGames.Combat
{
    public class DamageIndicator : MonoBehaviour
    {
        public Color normalTextColor = Color.white;
        public event Action<DamageIndicator> OnReturnToPool;

        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private Tween _moveXTween;
        private Tween _moveYTween;
        private Tween _scaleTween;
        private Tween _fadeTween;

        public void Show(int damageAmount, Vector3 position)
        {
            textMeshProUGUI.text = damageAmount.ToString();
            textMeshProUGUI.color = normalTextColor;
            transform.position = position;
            transform.localScale = Vector3.one;

            _moveXTween = transform.DOMoveX(transform.position.x + 0.5f, 0.5f).SetEase(Ease.InOutQuad);
            _moveYTween = transform.DOMoveY(transform.position.y + 0.2f, 0.5f).SetEase(Ease.OutQuad);

            _scaleTween = transform.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => FadeOut());
        }

        private void FadeOut()
        {
            _fadeTween = textMeshProUGUI.DOFade(0f, 0.7f).SetEase(Ease.OutQuad).OnComplete(() => ReturnToPool());
        }

        private void ReturnToPool()
        {
            OnReturnToPool?.Invoke(this);
        }

        private void OnDestroy()
        {
            _moveXTween?.Kill();
            _moveYTween?.Kill();
            _scaleTween?.Kill();
            _fadeTween?.Kill();
        }
    }
}