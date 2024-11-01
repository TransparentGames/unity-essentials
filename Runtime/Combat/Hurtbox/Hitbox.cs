using System;
using System.Collections;
using System.Collections.Generic;
using TransparentGames.Essentials.Combat;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public enum ColliderState
    {
        Closed,
        Open,
        Colliding
    }

    public class Hitbox : MonoBehaviour
    {
        public event Action<IHittable> HittableHit;

        [SerializeField] private LayerMask mask;
        [SerializeField] private Vector3 hitboxSize = Vector3.one;
        [SerializeField] private Color inactiveColor;
        [SerializeField] private Color collisionOpenColor;
        [SerializeField] private Color collidingColor;

        private ColliderState _state = ColliderState.Closed;
        private List<IHittable> _allHitHittables = new();

        public void HitboxUpdate()
        {
            if (_state == ColliderState.Closed)
                return;

            var hitHitables = HitboxTick();
            foreach (var hitHittable in hitHitables)
            {
                if (_allHitHittables.Contains(hitHittable))
                    continue;

                HittableHit?.Invoke(hitHittable);
                _allHitHittables.Add(hitHittable);
            }

            _state = hitHitables.Count > 0 ? ColliderState.Colliding : ColliderState.Open;
        }

        public List<IHittable> HitboxTick()
        {
            var hitHittables = new List<IHittable>();
            Collider[] hitColliders = new Collider[10];
            var count = Physics.OverlapBoxNonAlloc(
                transform.position,
                Vector3.Scale(hitboxSize, transform.lossyScale) / 2, // Divide by 2 to match Gizmo size
                hitColliders,
                transform.rotation, // Use transform.rotation to match Gizmo rotation
                mask
            );

            for (int i = 0; i < count; i++)
            {
                if (!hitColliders[i].TryGetComponent(out IHittable hittable))
                    continue;

                hitHittables.Add(hittable);
            }

            return hitHittables;
        }

        private void OnDrawGizmos()
        {
            CheckGizmoColor();
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, hitboxSize); // Use hitboxSize directly
        }

        public void StartCheckingCollision()
        {
            _allHitHittables.Clear();
            _state = ColliderState.Open;
        }

        public void StopCheckingCollision()
        {
            _state = ColliderState.Closed;
        }

        private void CheckGizmoColor()
        {
            switch (_state)
            {
                case ColliderState.Closed:
                    Gizmos.color = inactiveColor;
                    break;
                case ColliderState.Open:
                    Gizmos.color = collisionOpenColor;
                    break;
                case ColliderState.Colliding:
                    Gizmos.color = collidingColor;
                    break;
            }
        }
    }
}
