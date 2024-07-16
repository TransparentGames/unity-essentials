using System;
using UnityEngine;

namespace TransparentGames.Essentials.Stats
{
    [Serializable]
    public class Stat
    {
        public virtual float Value { get => _value ?? value; set => _value = value; }

        public StatDefinition statDefinition;
        [SerializeField] protected float value;

        protected float? _value;

        public Stat(StatDefinition statDefinition, float value)
        {
            this.statDefinition = statDefinition;
            _value = value;
        }

        public Stat(string key, float value)
        {
            // TODO get this value from a StatCollection
            //statDefinition = new StatDefinition(key);
            _value = value;
        }
    }
}