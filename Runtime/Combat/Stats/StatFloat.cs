using System;
using UnityEngine;

namespace TransparentGames.Combat
{
    [Serializable]
    public class StatFloat : Stat<float>
    {
        [SerializeField] private string name;
        [SerializeField] private float defaultValue;

        private float? _value;

        public StatFloat(string name, float defaultValue)
        {
            this.name = name;
            this.defaultValue = defaultValue;
        }

        public override string Name => name;
        public override float Value
        {
            get
            {
                if (!_value.HasValue)
                {
                    _value = defaultValue;
                }
                return _value.Value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
