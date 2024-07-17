using System;
using UnityEngine;

namespace TransparentGames.Essentials.Data
{
    [Serializable]
    public class DataInt : IDataProperty<int>
    {
        public event Action Changed;
        public string Key => key;

        [SerializeField] private int defaultValue;
        [SerializeField] private string key;

        private int? _value;

        public DataInt(string key, int initValue)
        {
            this.key = key;
            defaultValue = initValue;
        }

        public int Value
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
