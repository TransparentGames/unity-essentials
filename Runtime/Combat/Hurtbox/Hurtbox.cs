using System;
using System.Collections;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Stats;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    [RequireComponent(typeof(Collider))]
    public class Hurtbox : ComponentBase, IHittable
    {
        public Entity Owner => owner;
        public Transform Transform => transform;
        public event Action<HitResult> HitResultEvent;
        public event Action<HitInfo> HitInfoEvent;

        [SerializeField] private Color inactiveColor;
        [SerializeField] private Color collisionOpenColor;
        [SerializeField] private HurtboxDamage hurtboxDamage;

        private ColliderState _state = ColliderState.Open;
        private Collider _collider;

        public HitResult OnHit(HitInfo hitInfo)
        {
            var hitResult = new HitResult()
            {
                damageDealt = 0,
                wasKilled = false,
                hitObject = owner,
                isCritical = hitInfo.isCritical
            };

            if (_state == ColliderState.Closed || hurtboxDamage == null)
            {
                HitInfoEvent?.Invoke(hitInfo);
                return hitResult;
            }

            hitResult = hurtboxDamage.OnHit(hitResult, hitInfo);

            HitResultEvent?.Invoke(hitResult);
            HitInfoEvent?.Invoke(hitInfo);

            return hitResult;
        }

        public void StartInvincibility()
        {
            _state = ColliderState.Closed;
        }

        public void StopInvincibility()
        {
            _state = ColliderState.Open;
        }

        private void OnDrawGizmos()
        {
            if (_collider == null)
                _collider = GetComponent<Collider>();
            CheckGizmoColor();
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.DrawCube(_collider.bounds.center - transform.position, _collider.bounds.size);
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
            }
        }
    }
}
