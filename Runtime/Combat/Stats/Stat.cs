using System;
using UnityEngine;

namespace TransparentGames.Essentials.Stats
{
    [Serializable]
    public class Stat
    {
        public virtual float Value
        {
            get { return _value ?? value; }
            set { _value = value; }
        }
        /// <summary>
        /// The type of stat. Use it for identifying the stat in the database.
        /// </summary>
        public string Type => statDefinition.Type;
        /// <summary>
        /// The family type of the stat. Use it for identifying the family of the stat in the database.
        /// </summary>
        public string FamilyType => statDefinition.FamilyType;
        public string DisplayValue => statDefinition.GetDisplayValue(Value);
        public string DisplayName => statDefinition.GetDisplayName();

        [SerializeField] private StatDefinition statDefinition;
        [SerializeField] protected float value;

        protected float? _value;

        public Stat(StatDefinition statDefinition, float value)
        {
            this.statDefinition = statDefinition != null ? statDefinition : throw new ArgumentNullException(nameof(statDefinition));
            this.value = value;
            _value = value;
        }

        public Stat(Stat stat)
        {
            if (stat == null) throw new ArgumentNullException(nameof(stat));
            statDefinition = stat.statDefinition;
            _value = stat.Value;
        }

        public float Calculate(float baseValue = 1)
        {
            return statDefinition.Calculate(Value, baseValue);
        }
    }
}