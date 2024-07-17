using System;
using UnityEngine;

namespace TransparentGames.Essentials.Data
{
    [Serializable]
    public class DataFloat : IDataProperty<float>
    {
        public event Action Changed;
        public string Key => key;

        [SerializeField] private float defaultValue;
        [SerializeField] private string key;

        private float? _value;

        public DataFloat(string key, float initValue)
        {
            this.key = key;
            defaultValue = initValue;
        }

        public float Value
        {
            get
            {
                if (_value.HasValue)
                    return _value.Value;

                _value = defaultValue;
                return _value.Value;
            }
            set
            {
                if (Value != value)
                {
                    _value = value;
                    Save();
                }
            }
        }

        public void Save()
        {
            Changed?.Invoke();
        }
    }
}
