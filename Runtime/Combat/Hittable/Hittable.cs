using System;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Stats;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    [RequireComponent(typeof(IHealth))]
    public class Hittable : MonoBehaviour, IHittable, IStatsRequired, IComponent
    {
        public GameObject Owner { get; set; }
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

            var dmgReduction = _defense / (_defense + (5 * hitInfo.level) + 500);
            var damage = Mathf.CeilToInt(hitInfo.damage * (1 - dmgReduction));

            _health.Add(-damage);
            HitResult hitResult = new()
            {
                damageDealt = (int)damage,
                wasKilled = _health.CurrentHealth <= 0,
                hitObject = Owner
            };

            HitResultEvent?.Invoke(hitResult);
            HitInfoEvent?.Invoke(hitInfo);

            return hitResult;
        }

        public void OnStatsChanged(StatsHolder statsHolder)
        {
            if (statsHolder.Stats.TryGetValue("Defense", out Stat defenseStat))
            {
                _defense = defenseStat.Value;
            }
        }
    }
}