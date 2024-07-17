using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.Data
{
    public class PlayerDataManager : PersistentMonoSingleton<PlayerDataManager>
    {
        public event Action<KeyValuePair<string, string>> Changed;

        public void AddPlayerData<T>(KeyValuePair<string, T> data) where T : class, new()
        {
            if (CachedDataManager.Instance.TryGetProperty<string>(data.Key, out var value))
            {
                Debug.Log($"PlayerData [{data.Key}] already exists in inventory, updating...");
                UpdatePlayerData(data.Key, data.Value);
                return;
            }

            CreatePlayerData<T>(data.Key);
        }

        public IDataProperty<string> GetPlayerData<T>(string key) where T : class, new()
        {
            if (CachedDataManager.Instance.TryGetProperty<string>(key, out var playerData))
            {
                return playerData;
            }

            CreatePlayerData<T>(key);
            return GetPlayerData<T>(key);
        }

        public IDataProperty<string> GetPlayerData(string key, string defaultValue)
        {
            if (CachedDataManager.Instance.TryGetProperty<string>(key, out var playerData))
            {
                return CachedDataManager.Instance.GetProperty(key, defaultValue);
            }

            CreatePlayerData(key, defaultValue);
            return GetPlayerData(key, defaultValue);
        }

        public void CreatePlayerData<T>(string key) where T : class, new()
        {
            string value = CachedDataManager.Instance.GetProperty(key, JsonConvert.SerializeObject(new T())).Value;

            Debug.Log($"Created playerData [{key}]");
            Changed?.Invoke(new KeyValuePair<string, string>(key, value));
        }

        public void CreatePlayerData(string key, string defaultValue)
        {
            string value = CachedDataManager.Instance.GetProperty(key, defaultValue).Value;

            Debug.Log($"Created playerData [{key}]");
            Changed?.Invoke(new KeyValuePair<string, string>(key, value));
        }

        public void UpdatePlayerData<T>(string key, T data) where T : class, new()
        {
            if (!CachedDataManager.Instance.TryGetProperty<string>(key, out var playerData))
                return;

            playerData.Value = JsonConvert.SerializeObject(data);

            Debug.Log($"Updated playerData [{key}]");
            Changed?.Invoke(new KeyValuePair<string, string>(key, playerData.Value));
        }
    }
}