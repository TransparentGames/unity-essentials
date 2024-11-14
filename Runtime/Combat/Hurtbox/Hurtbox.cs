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
    public class Hurtbox : ComponentBase, IHittable, IHurtboxComponent
    {
        public Entity Owner => owner;
        public Transform Transform => transform;

        public DamagePhase Phase => DamagePhase.PreCalculation;

        public event Action<HitResult> HitResultEvent;
        public event Action<HitInfo> HitInfoEvent;

        [SerializeField] private Color inactiveColor;
        [SerializeField] private Color collisionOpenColor;

        private ColliderState _state = ColliderState.Open;
        private Collider _collider;
        private List<IHurtboxComponent> _hurtboxComponents;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _hurtboxComponents = new List<IHurtboxComponent>(GetComponents<IHurtboxComponent>());
            _hurtboxComponents.Sort((a, b) => a.Phase.CompareTo(b.Phase));
        }

        public HitResult OnHit(HitInfo hitInfo)
        {
            var hitResult = new HitResult()
            {
                damageDealt = 0,
                wasKilled = false,
                hitObject = owner,
                isCritical = hitInfo.isCritical
            };

            foreach (var hurtboxComponent in _hurtboxComponents)
                if (hurtboxComponent.HandleHit(ref hitInfo) == false)
                    break;

            HitInfoEvent?.Invoke(hitInfo);

            if (hitInfo.damage > 0)
            {
                hitResult.damageDealt = hitInfo.damage;
                HitResultEvent?.Invoke(hitResult);
            }

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

        public bool HandleHit(ref HitInfo hitInfo)
        {
            if (_state == ColliderState.Closed)
            {
                hitInfo.damage = 0;
                return false;
            }

            return true;
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
