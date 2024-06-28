using System;
using UnityEngine;

namespace TransparentGames.UI
{
    public class DynamicUiElement : MonoBehaviour
    {
        [SerializeField] private bool clampToScreenEdge;
        [SerializeField] private float screenOffset = .05f;
        [SerializeField] private bool smoothMovement = false;
        [SerializeField] private bool separateOffsets = false;
        [SerializeField] private float screenOffsetX = .05f;
        [SerializeField] private float screenOffsetY = .05f;

        private Camera mainCamera;
        protected RectTransform rectTransform;

        private bool isInited;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;

            Init();
            isInited = true;
        }

        private void OnEnable()
        {
            mainCamera = Camera.main;
        }

        protected virtual void Init()
        {
        }

        public void UpdatePosition(Vector3 worldPosition)
        {
            if (!isInited) return;

            var newAnchor = mainCamera.WorldToViewportPoint(worldPosition);

            // Hide the UI element if it's outside the screen
            if (newAnchor.z < 0)
            {
                newAnchor.x = -1;
                newAnchor.y = -1;
            }

            if (clampToScreenEdge)
            {
                if (separateOffsets)
                {
                    var minX = 0 + screenOffsetX;
                    var maxX = 1 - screenOffsetX;

                    var minY = 0 + screenOffsetY;
                    var maxY = 1 - screenOffsetY;

                    newAnchor.x = Mathf.Clamp(newAnchor.x, minX, maxX);
                    newAnchor.y = Mathf.Clamp(newAnchor.y, minY, maxY);
                }
                else
                {
                    var min = 0 + screenOffset;
                    var max = 1 - screenOffset;

                    newAnchor.x = Mathf.Clamp(newAnchor.x, min, max);
                    newAnchor.y = Mathf.Clamp(newAnchor.y, min, max);
                }
            }

            if (smoothMovement)
            {
                newAnchor = Vector3.Lerp(rectTransform.anchorMin, newAnchor, .1f);
            }

            rectTransform.anchorMin = newAnchor;
            rectTransform.anchorMax = newAnchor;
        }

        private void LateUpdate()
        {
            AlignWithCamera();
        }

        private void AlignWithCamera()
        {
            // Make sure the UI element stays aligned with the camera
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.position);
            transform.position = mainCamera.ScreenToWorldPoint(screenPoint);

            // Ensure the UI element is facing the correct direction in screen space
            Vector3 direction = mainCamera.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
        }
    }
}
