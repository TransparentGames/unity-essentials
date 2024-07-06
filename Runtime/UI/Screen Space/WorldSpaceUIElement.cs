using UnityEngine;

namespace TransparentGames.UI.ScreenSpace
{
    [RequireComponent(typeof(CanvasGroup), typeof(DynamicUiElement))]
    public class WorldSpaceUIElement : MonoBehaviour
    {
        [SerializeField]
        private Vector3 offset;

        private Transform _followTarget;
        private CanvasGroup _canvasGroup;
        private DynamicUiElement _dynamicUiElement;
        private RectTransform _rectTransform;

        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _dynamicUiElement = GetComponent<DynamicUiElement>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void LateUpdate()
        {
            if (_followTarget == null) return;
            if (gameObject.activeSelf == false) return;

            _dynamicUiElement.UpdatePosition(_followTarget.position + offset);
        }

        public void SetOpacity(float newOpacity)
        {
            _canvasGroup.alpha = newOpacity;
        }

        public void SetTarget(Transform target)
        {
            _followTarget = target;
        }

        public void SetOffset(Vector3 offset)
        {
            this.offset = offset;
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}