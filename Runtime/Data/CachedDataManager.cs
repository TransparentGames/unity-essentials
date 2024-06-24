using System.Collections.Generic;
using PlayFab.ClientModels;
using TransparentGames.Essentials.PlayFab;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Data
{
    //[CreateAssetMenu(fileName = "Cached Data Manager", menuName = "Transparent Games/Data/Cached Data Manager", order = 0)]
    public class CachedDataManager : ScriptableObjectSingleton<CachedDataManager>, IDataSaveManager
    {
        public Dictionary<string, object> Properties => _properties;
        private static Dictionary<string, object> _properties = new();

        protected override void OnInitializing()
        {
            base.OnInitializing();
            _properties.Clear();
        }

        public IDataProperty<int> GetProperty(string key, int defaultValue)
        {
            if (_properties.TryGetValue(key, out var property))
                return (IDataProperty<int>)property;

            var newProperty = new DataInt(key, defaultValue);
            _properties.Add(key, newProperty);
            return newProperty;
        }

        public IDataProperty<bool> GetProperty(string key, bool defaultValue)
        {
            if (_properties.TryGetValue(key, out var property))
                return (IDataProperty<bool>)property;

            var newProperty = new DataBool(key, defaultValue);
            _properties.Add(key, newProperty);
            return newProperty;
        }

        public IDataProperty<float> GetProperty(string key, float defaultValue)
        {
            if (_properties.TryGetValue(key, out var property))
                return (IDataProperty<float>)property;

            var newProperty = new DataFloat(key, defaultValue);
            _properties.Add(key, newProperty);
            return newProperty;
        }

        public IDataProperty<string> GetProperty(string key, string defaultValue)
        {
            if (_properties.TryGetValue(key, out var property))
                return (IDataProperty<string>)property;

            var newProperty = new DataString(key, defaultValue);
            _properties.Add(key, newProperty);
            return newProperty;
        }

        public bool TryGetProperty<T>(string key, out IDataProperty<T> value)
        {
            if (_properties.TryGetValue(key, out var property))
            {
                value = (IDataProperty<T>)property;
                return true;
            }

            value = null;
            return false;
        }
    }
}