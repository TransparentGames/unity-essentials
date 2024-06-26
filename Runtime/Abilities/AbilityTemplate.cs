using UnityEngine;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Stats;

namespace TransparentGames.Abilities
{
    [CreateAssetMenu(fileName = "New Ability Template", menuName = "Transparent Games/Abilities/Ability Template")]
    public class AbilityTemplate : ScriptableObjectWithId
    {
        [Space]
        public string abilityName;
        public Ability abilityPrefab;
        public List<Stat> multipliers;
        public LayerMask layerMask;

        public virtual float Calculate(List<Stat> stats)
        {
            float result = 0f;
            foreach (Stat stat in multipliers)
            {
                result += stats.Find(s => s.statDefinition.statName == stat.statDefinition.statName).value * stat.value;
            }
            return result;
        }

        public virtual bool CanUse(Caster caster)
        {
            return true;
        }
    }
}