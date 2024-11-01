using TransparentGames.Essentials;
using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Stats;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public class HurtboxDamage : ComponentBase, IStatsRequired
    {
        public Entity Owner => owner;

        [SerializeField] private StatDefinition defenseStatDefinition = null;

        private IHealth _health;
        private float _defense = 0;

        private void Start()
        {
            _health = owner.GetComponent<IHealth>();
        }

        public HitResult OnHit(HitResult hitResult, HitInfo hitInfo)
        {
            if (_health.CurrentHealth <= 0)
                return hitResult;

            var dmgReduction = _defense / (_defense + (5 * hitInfo.level) + 500);
            var damage = Mathf.CeilToInt(hitInfo.damage * (1 - dmgReduction));

            hitResult.damageDealt = (int)damage;
            hitResult.wasKilled = _health.CurrentHealth - damage <= 0;

            _health.Add(-damage);

            return hitResult;
        }

        public void OnStatsChanged(StatsHolder statsHolder)
        {
            if (statsHolder.Stats.TryGetValue(defenseStatDefinition.Type, out Stat defenseStat))
            {
                _defense = defenseStat.Value;
            }
        }
    }
}