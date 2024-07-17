using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.Data
{
    public class RemoteConfigManager : PersistentMonoSingleton<RemoteConfigManager>
    {
        public event Action Changed;
        private const string _REMOTE_CONFIG_PREFIX = "remote_";

        public void AddPlayerData<T>(KeyValuePair<string, T> data) where T : class, new()
        {
            string prefixedKey = _REMOTE_CONFIG_PREFIX + data.Key;
            if (CachedDataManager.Instance.TryGetProperty<string>(prefixedKey, out var value))
            {
                Debug.LogError($"RemoteConfig [{prefixedKey}] already exists in inventory.");
                return;
            }

            CreatePlayerData<T>(prefixedKey);
        }

        public void AddRemoteConfig(string key, string defaultValue)
        {
            string prefixedKey = _REMOTE_CONFIG_PREFIX + key;
            if (CachedDataManager.Instance.TryGetProperty<string>(prefixedKey, out var remoteConfig))
            {
                Debug.LogError($"RemoteConfig [{prefixedKey}] already exists in inventory.");
                return;
            }

            CreatePlayerData(prefixedKey, defaultValue);
        }

        public T GetRemoteConfig<T>(string key) where T : class, new()
        {
            string prefixedKey = _REMOTE_CONFIG_PREFIX + key;
            if (CachedDataManager.Instance.TryGetProperty<string>(prefixedKey, out var remoteConfig))
            {
                return JsonConvert.DeserializeObject<T>(remoteConfig.Value);
            }

            CreatePlayerData<T>(prefixedKey);
            return GetRemoteConfig<T>(key);
        }

        public bool TryGetRemoteConfig<T>(string key, out T remoteConfig) where T : class, new()
        {
            string prefixedKey = _REMOTE_CONFIG_PREFIX + key;
            if (CachedDataManager.Instance.TryGetProperty<string>(prefixedKey, out var remoteConfigJson))
            {
                remoteConfig = JsonConvert.DeserializeObject<T>(remoteConfigJson.Value);
                return true;
            }

            remoteConfig = null;
            return false;
        }

        public IDataProperty<string> GetPlayerData<T>(string key) where T : class, new()
        {
            string prefixedKey = _REMOTE_CONFIG_PREFIX + key;
            if (CachedDataManager.Instance.TryGetProperty<string>(prefixedKey, out var playerData))
            {
                return playerData;
            }

            CreatePlayerData<T>(prefixedKey);
            return GetPlayerData<T>(key);
        }

        public IDataProperty<string> GetPlayerData(string key, string defaultValue)
        {
            key = _REMOTE_CONFIG_PREFIX + key;
            if (CachedDataManager.Instance.TryGetProperty<string>(key, out var playerData))
            {
                return CachedDataManager.Instance.GetProperty(key, defaultValue);
            }

            CreatePlayerData(key, defaultValue);
            return GetPlayerData(key, defaultValue);
        }

        private void CreatePlayerData<T>(string key) where T : class, new()
        {
            string value = CachedDataManager.Instance.GetProperty(key, JsonConvert.SerializeObject(new T())).Value;

            Debug.Log($"Created playerData [{key}]");
            OnChanged(new KeyValuePair<string, string>(key, value));
        }

        private void CreatePlayerData(string key, string defaultValue)
        {
            string value = CachedDataManager.Instance.GetProperty(key, defaultValue).Value;

            Debug.Log($"Created playerData [{key}]");
            OnChanged(new KeyValuePair<string, string>(key, value));
        }

        private void OnChanged(KeyValuePair<string, string> data)
        {
            Changed?.Invoke();
        }
    }
}