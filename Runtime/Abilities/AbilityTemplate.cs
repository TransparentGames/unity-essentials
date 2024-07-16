using UnityEngine;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Stats;

namespace TransparentGames.Abilities
{
    [CreateAssetMenu(fileName = "New Ability Template", menuName = "Transparent Games/Abilities/Ability Template")]
    public class AbilityTemplate : ScriptableObjectWithId
    {
        [Space]
        public string abilityName;
        public Ability abilityPrefab;
        public List<AdditionalStat> additionalStats;
        public LayerMask layerMask;

        public virtual float Calculate(Dictionary<string, Stat> stats)
        {
            float result = 0f;
            foreach (var additionalStat in additionalStats)
            {
                if (stats.TryGetValue(additionalStat.statDefinition.statName, out Stat existingStat))
                {
                    result += additionalStat.Calculate(existingStat.Value);
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