using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.Data
{
    public class GameplayDataSaveManager : MonoSingleton<GameplayDataSaveManager>, IDataSaveManager
    {
        public Dictionary<string, object> Properties => _properties;
        private static readonly Dictionary<string, object> _properties = new();

        protected override void OnInitializing()
        {
            base.OnInitializing();
            _properties.Clear();
        }

        public IDataProperty<T> GetProperty<T>(string key, T defaultValue)
        {
            if (_properties.TryGetValue(key, out var property))
                return (IDataProperty<T>)property;

            IDataProperty<T> newProperty = defaultValue switch
            {
                int intValue => (IDataProperty<T>)new DataInt(key, intValue),
                bool boolValue => (IDataProperty<T>)new DataBool(key, boolValue),
                float floatValue => (IDataProperty<T>)new DataFloat(key, floatValue),
                string stringValue => (IDataProperty<T>)new DataString(key, stringValue),
                _ => throw new System.ArgumentException($"Unsupported type: {typeof(T)}")
            };

            _properties.Add(key, newProperty);
            return newProperty;
        }

        public bool TryGetProperty<T>(string key, out IDataProperty<T> value)
        {
            if (_properties.TryGetValue(key, out var property) && property is IDataProperty<T> typedProperty)
            {
                value = typedProperty;
                return true;
            }

            value = null;
            return false;
        }

        public override void ClearSingleton()
        {
            _properties.Clear();
            base.ClearSingleton();
        }
    }
}