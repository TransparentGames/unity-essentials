using System;
using UnityEngine;

namespace TransparentGames.Essentials.UI.ScreenSpace
{
    public class ScreenSpaceUiElement : MonoBehaviour
    {
        [SerializeField] private bool clampToScreenEdge;
        [SerializeField] private float screenOffset = .05f;
        [SerializeField] private bool smoothMovement = false;
        [SerializeField] private bool separateOffsets = false;
        [SerializeField] private float screenOffsetX = .05f;
        [SerializeField] private float screenOffsetY = .05f;
        [SerializeField] private Vector3 offset;

        private Camera uiCamera;
        private RectTransform rectTransform;
        private bool isInited;
        private Transform followTarget;
        private CanvasGroup canvasGroup;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform.anchoredPosition = Vector2.zero;
            Init();
            isInited = true;
        }

        private void OnEnable()
        {
            uiCamera = UICamera.Instance.Camera;
        }

        protected virtual void Init() { }

        public void UpdatePosition(Vector3 worldPosition)
        {
            if (!isInited) return;

            var newAnchor = uiCamera.WorldToViewportPoint(worldPosition);

            if (newAnchor.z < 0)
            {
                newAnchor.x = -1;
                newAnchor.y = -1;
            }

            if (clampToScreenEdge)
            {
                if (separateOffsets)
                {
                    newAnchor.x = Mathf.Clamp(newAnchor.x, screenOffsetX, 1 - screenOffsetX);
                    newAnchor.y = Mathf.Clamp(newAnchor.y, screenOffsetY, 1 - screenOffsetY);
                }
                else
                {
                    newAnchor.x = Mathf.Clamp(newAnchor.x, screenOffset, 1 - screenOffset);
                    newAnchor.y = Mathf.Clamp(newAnchor.y, screenOffset, 1 - screenOffset);
                }
            }

            if (smoothMovement)
            {
                newAnchor = Vector3.Lerp(rectTransform.anchorMin, newAnchor, 0.1f);
            }

            rectTransform.anchorMin = newAnchor;
            rectTransform.anchorMax = newAnchor;
        }

        private void LateUpdate()
        {
            if (followTarget != null && gameObject.activeSelf)
            {
                UpdatePosition(followTarget.position + offset);
            }
        }


        public void SetOpacity(float newOpacity)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = newOpacity;
            }
        }

        public void SetTarget(Transform target)
        {
            followTarget = target;
        }

        public void SetOffset(Vector3 newOffset)
        {
            offset = newOffset;
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
