using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransparentGames.Essentials.Singletons;
using UnityEngine;
using TransparentGames.Essentials.Data;

namespace TransparentGames.Essentials.Currency
{
    /// <summary>
    /// Manages the player's currency.
    /// </summary>
    public class CurrencyManager : PersistentMonoSingleton<CurrencyManager>
    {
        public event Action Changed;
        private const string _DATA_PREFIX = "currency_";

        public void AddCurrency(string key, int defaultValue)
        {
            string prefixedKey = _DATA_PREFIX + key;
            if (CachedDataManager.Instance.TryGetProperty<int>(prefixedKey, out var remoteConfig))
            {
                Debug.LogError($"Currency [{prefixedKey}] already exists in inventory.");
                UpdateCurrency(prefixedKey, defaultValue);
                return;
            }

            CreatePlayerData(prefixedKey, defaultValue);
        }

        public IDataProperty<int> Get(string key, int defaultValue)
        {
            string prefixedKey = _DATA_PREFIX + key;
            if (CachedDataManager.Instance.TryGetProperty<int>(prefixedKey, out var playerData))
            {
                return CachedDataManager.Instance.GetProperty(prefixedKey, defaultValue);
            }

            CreatePlayerData(prefixedKey, defaultValue);
            return Get(key, defaultValue);
        }

        private void UpdateCurrency(string key, int value)
        {
            if (!CachedDataManager.Instance.TryGetProperty<int>(key, out var playerData))
                return;

            playerData.Value = value;
            Debug.Log($"Updated playerData [{key}]");
            OnChanged();
        }

        private void CreatePlayerData(string key, int defaultValue)
        {
            CachedDataManager.Instance.GetProperty(key, defaultValue);

            Debug.Log($"Created playerData [{key}]");
            OnChanged();
        }

        private void OnChanged()
        {
            Changed?.Invoke();
        }

        public override void ClearSingleton()
        {
            base.ClearSingleton();
        }
    }
}