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
        public string Type => statDefinition.statName;
        public string DisplayValue => statDefinition.GetDisplayValue(Value);

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
    }
}