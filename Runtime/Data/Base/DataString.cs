using System;
using UnityEngine;

namespace TransparentGames.Essentials.Data
{
    [Serializable]
    public class DataString : IDataProperty<string>
    {
        public event Action Changed;
        public string Key => key;

        [SerializeField] private string defaultValue;
        [SerializeField] private string key;

        private string _value;

        public DataString(string key, string initValue)
        {
            this.key = key;
            defaultValue = initValue;
        }

        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(_value) == false)
                    return _value;

                _value = defaultValue;
                return _value;
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
