using TransparentGames.Essentials;
using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Stats;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public class HurtboxDamage : ComponentBase, IStatsRequired, IHurtboxComponent
    {
        public Entity Owner => owner;
        public DamagePhase Phase => DamagePhase.DamageCalculation;

        [SerializeField] private StatDefinition defenseStatDefinition = null;

        private float _defense = 0;

        public bool HandleHit(ref HitInfo hitInfo)
        {
            var dmgReduction = _defense / (_defense + (5 * hitInfo.level) + 500);
            var damage = Mathf.Floor(hitInfo.damage * (1 - dmgReduction));

            hitInfo.damage = damage;

            return true;
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