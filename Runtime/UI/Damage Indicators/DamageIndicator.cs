using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public class DamageIndicator : MonoBehaviour
    {
        public Color normalTextColor = Color.white;
        public Color criticalTextColor = Color.red;
        public event Action<DamageIndicator> OnReturnToPool;

        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private Tween _moveXTween;
        private Tween _moveYTween;
        private Tween _scaleTween;
        private Tween _fadeTween;

        public void Set(HitResult hitResult)
        {
            _moveXTween?.Kill();
            _moveYTween?.Kill();
            _scaleTween?.Kill();
            _fadeTween?.Kill();

            transform.localScale = Vector3.zero;

            textMeshProUGUI.text = hitResult.damageDealt.ToString();
            if (hitResult.isCritical)
                textMeshProUGUI.text += "!";
            textMeshProUGUI.color = hitResult.isCritical ? criticalTextColor : normalTextColor;

            _moveXTween = textMeshProUGUI.transform.DOMoveX(transform.position.x + 0.5f, 0.5f).SetEase(Ease.InOutQuad);
            _moveYTween = textMeshProUGUI.transform.DOMoveY(transform.position.y + 0.2f, 0.5f).SetEase(Ease.OutQuad);

            _scaleTween = transform.DOScale(Vector3.one * 5.0f, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => ScaleDown());
        }

        private void ScaleDown()
        {
            _scaleTween = transform.DOScale(Vector3.one * 2.0f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() => FadeOut());
        }

        private void FadeOut()
        {
            _fadeTween = textMeshProUGUI.DOFade(0f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => ReturnToPool());
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