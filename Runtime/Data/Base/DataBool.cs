using System;
using UnityEngine;

namespace TransparentGames.Data
{
    [Serializable]
    public class DataBool : IDataProperty<bool>
    {
        public event Action Changed;
        public string Key => key;

        [SerializeField] private bool defaultValue;
        [SerializeField] private string key;

        private bool? _value;

        public DataBool(string key, bool initValue)
        {
            this.key = key;
            defaultValue = initValue;
        }

        public bool Value
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
