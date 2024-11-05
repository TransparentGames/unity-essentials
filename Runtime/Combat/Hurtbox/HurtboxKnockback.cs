using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public class HurtboxKnockback : ComponentBase, IHurtboxComponent
    {
        private IMovable _movable;

        private void Awake()
        {
            _movable = owner.GetComponentInChildren<IMovable>();
        }

        public HitResult OnHit(HitResult hitResult, HitInfo hitInfo)
        {
            if (_movable != null)
            {
                var knockbackDirection = (owner.transform.position - hitInfo.source.transform.position).normalized * hitInfo.knockback * 0.1f;
                _movable.AddForce(knockbackDirection);
            }

            return hitResult;
        }
    }
}