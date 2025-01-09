using UnityEngine;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Stats;
using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Items;

namespace TransparentGames.Essentials.Abilities
{
    [CreateAssetMenu(fileName = "New Ability Template", menuName = "Transparent Games/Abilities/Ability Template")]
    public class AbilityTemplate : ItemTemplate
    {
        [Space]
        public Ability abilityPrefab;
        public List<Stat> modifiers;
        public LayerMask layerMask;
        public float cooldown;
        public int knockback;

        [SerializeField] private bool canCrit;

        public virtual HitInfo Calculate(Dictionary<string, Stat> stats)
        {
            var result = new HitInfo
            {
                damage = 0,
                isCritical = false,
                knockback = knockback
            };

            foreach (var modifier in modifiers)
            {
                if (stats.TryGetValue(modifier.FamilyType, out Stat existingStat))
                {
                    result.damage += modifier.Calculate(existingStat.Value);
                }
            }

            if (canCrit == false)
                return result;

            if (stats.TryGetValue("cri", out Stat ccStat) && stats.TryGetValue("cri_dmg", out Stat cdStat))
            {
                if (Random.value < ccStat.Value)
                {
                    result.damage *= cdStat.Value;
                    result.isCritical = true;
                }
            }

            return result;
        }
    }
}