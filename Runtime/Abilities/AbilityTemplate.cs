using UnityEngine;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Stats;
using TransparentGames.Essentials.Combat;

namespace TransparentGames.Essentials.Abilities
{
    [CreateAssetMenu(fileName = "New Ability Template", menuName = "Transparent Games/Abilities/Ability Template")]
    public class AbilityTemplate : ScriptableObjectWithId
    {
        [Space]
        public string abilityName;
        public Ability abilityPrefab;
        public List<AdditionalStat> additionalStats;
        public LayerMask layerMask;

        [SerializeField] private bool canCrit;

        public virtual HitInfo Calculate(Dictionary<string, Stat> stats)
        {
            var result = new HitInfo
            {
                damage = 0,
                isCritical = false
            };

            foreach (var additionalStat in additionalStats)
            {
                if (stats.TryGetValue(additionalStat.statDefinition.statName, out Stat existingStat))
                {
                    result.damage += additionalStat.Calculate(existingStat.Value);
                }
            }

            if (canCrit == false)
                return result;

            if (stats.TryGetValue("Critical Chance", out Stat ccStat) && stats.TryGetValue("Critical Damage", out Stat cdStat))
            {
                if (Random.value < ccStat.Value)
                {
                    result.damage *= cdStat.Value;
                    result.isCritical = true;
                }
            }

            return result;
        }

        public virtual bool CanUse(Caster caster)
        {
            return true;
        }
    }
}