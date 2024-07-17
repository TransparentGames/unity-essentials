using TransparentGames.Essentials.UpdateManagement;
using UnityEngine;

namespace TransparentGames.Essentials.UI.WorldSpace
{
    public class WorldSpaceElement : MonoBehaviour
    {
        private Transform _followTarget;
        private Vector3 _offset = Vector3.zero;

        public void SetTarget(Transform target)
        {
            _followTarget = target;
            gameObject.transform.position = _followTarget.position + _offset;
        }

        public void SetOffset(Vector3 offset)
        {
            _offset = offset;
        }

        private void LateUpdate()
        {
            if (_followTarget == null) return;
            if (gameObject.activeSelf == false) return;

            var newPosition = _followTarget.position + _offset;

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPosition, 0.1f);
            gameObject.transform.forward = Camera.main.transform.forward;
        }
    }
}