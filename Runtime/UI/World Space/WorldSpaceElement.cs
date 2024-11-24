using TransparentGames.Essentials.UpdateManagement;
using UnityEngine;

namespace TransparentGames.Essentials.UI.WorldSpace
{
    public class WorldSpaceElement : MonoBehaviour
    {
        [SerializeField] private float lerpTime = 0.1f;

        private Transform _followTarget;
        private Vector3 _offset = Vector3.zero;
        private Transform _transform;

        private void Awake()
        {
            // Cache the transform component
            _transform = transform;
        }

        public void SetTarget(Transform target)
        {
            _followTarget = target;
            _transform.position = _followTarget.position + _offset;
        }

        public void SetOffset(Vector3 offset)
        {
            _offset = offset;
        }

        private void LateUpdate()
        {
            if (_followTarget == null || !gameObject.activeSelf) return;

            // Calculate the new position with the offset
            var newPosition = _followTarget.position + _offset;

            // Smoothly interpolate to the new position
            _transform.position = Vector3.Lerp(_transform.position, newPosition, lerpTime);

            // Ensure the element faces the camera
            if (Camera.main != null)
            {
                _transform.forward = Camera.main.transform.forward;
            }
        }
    }
}