using System;
using TransparentGames.Essentials.Combat;
using TransparentGames.Stats;
using UnityEngine;

namespace TransparentGames.Combat
{
    [RequireComponent(typeof(IHealth))]
    public class Hittable : MonoBehaviour, IHittable, IStatsRequired
    {
        public GameObject GameObject => gameObject;
        public StatsHolder StatsHolder { get; set; }

        public event Action<HitResult> HitResultEvent;
        public event Action<HitInfo> HitInfoEvent;

        private IHealth _health;
        private bool _isInvincible = false;
        private float _defense = 0;

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

            var damage = Mathf.CeilToInt(hitInfo.damage * (1 - (_defense / (_defense + 5 * hitInfo.level + 500))));

            _health.Add(-damage);
            HitResult hitResult = new()
            {
                damageDealt = (int)damage,
                wasKilled = _health.CurrentHealth <= 0,
                hitObject = GameObject
            };

            HitResultEvent?.Invoke(hitResult);
            HitInfoEvent?.Invoke(hitInfo);

            return hitResult;
        }

        public void OnStatsChanged()
        {
            if (StatsHolder.Stats.TryGetValue("Defense", out Stat defenseStat))
            {
                _defense = defenseStat.value;
            }
        }
    }
}