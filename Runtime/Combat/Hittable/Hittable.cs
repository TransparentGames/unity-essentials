using System;
using TransparentGames.Essentials.Combat;
using UnityEngine;

namespace TransparentGames.Combat
{
    [RequireComponent(typeof(IHealth))]
    public class Hittable : MonoBehaviour, IHittable
    {
        public GameObject GameObject => gameObject;

        public event Action<HitResult> HitResultEvent;
        public event Action<HitInfo> HitInfoEvent;

        private IHealth _health;
        private bool _isInvincible = false;

        private void Awake()
        {
            _health = GetComponent<IHealth>();
        }

        public void SetInvincible(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }

        public HitResult OnHit(HitInfo hitInfo)
        {
            if (_isInvincible)
                return new HitResult();

            if (_health.CurrentHealth <= 0)
                return new HitResult();

            _health.Add(-hitInfo.damage);

            HitResult hitResult = new()
            {
                damageDealt = (int)hitInfo.damage,
                wasKilled = _health.CurrentHealth <= 0,
                hitObject = GameObject
            };

            HitResultEvent?.Invoke(hitResult);
            HitInfoEvent?.Invoke(hitInfo);

            return hitResult;
        }
    }
}