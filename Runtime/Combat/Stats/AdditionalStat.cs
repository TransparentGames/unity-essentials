using System;
using UnityEngine;

namespace TransparentGames.Essentials.Stats
{
    [Serializable]
    public class AdditionalStat : Stat
    {
        [SerializeField] private bool isPercentage = false;

        public AdditionalStat(StatDefinition statDefinition, float value, bool isPercentage = false) : base(statDefinition, value)
        {
            this.isPercentage = isPercentage;
        }

        public float Calculate(float baseValue)
        {
            var calculatedValue = isPercentage ? baseValue * Value : Value;
            return calculatedValue;
        }
    }
}