using System.Collections.Generic;
using UnityEngine;
using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Detection;
using System;

namespace TransparentGames.Abilities
{
    public class GenericAbility : Ability, IAbilityCallbacks
    {
        [SerializeField] private Vector2 hitboxSize = new(1, 3);

        private List<HitResult> _hitResults = new();

        public override void Use(Caster caster)
        {
            throw new NotImplementedException();
        }

        public void OnDealDamage()
        {
            Vector2 localHitboxSize = hitboxSize * transform.localScale;
            List<IDetectable> detected = DetectableRegistry.Instance.OverlapBox(transform.position, localHitboxSize, Quaternion.Euler(0, 0, transform.eulerAngles.z), LayerMask);
            foreach (IDetectable detectable in detected)
            {
                if (detectable.Owner == Owner)
                    continue;

                if (detectable.Owner.TryGetComponent(out IHittable hittable))
                {
                    _hitResults.Add(hittable.OnHit(new HitInfo { damage = Damage, source = Owner }));
                }
            }

            OnHitResults(_hitResults);
        }

        public void OnAnimationEnd()
        {
            OnFinished();
        }

        private void OnDrawGizmos()
        {
            Vector2 localHitboxSize = hitboxSize * transform.localScale;

            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z), Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, localHitboxSize);
        }


    }
}