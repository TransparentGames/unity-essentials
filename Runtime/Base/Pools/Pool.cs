using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransparentGames.Essentials.Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TransparentGames.Essentials.Pools
{
    public class Pool : MonoSingleton<Pool>
    {
        private readonly Dictionary<string, object> _poolHandlers = new();

        public async Task Preload<T>(List<IPoolData> poolDataList, Transform parent = null) where T : Component
        {
            foreach (var poolData in poolDataList)
            {
                if (_poolHandlers.ContainsKey(poolData.Name))
                    continue;

                await PreloadPool<T>(poolData, parent);
            }
        }

        private async Task PreloadPool<T>(IPoolData poolData, Transform parent) where T : Component
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(poolData.Reference);
            await handle.Task;

            if (handle.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load asset for pool: {poolData.Name}");
                return;
            }

            var templateGameObject = handle.Result;
            if (templateGameObject.TryGetComponent<T>(out var component) == false)
            {
                Debug.LogError($"No component of type {typeof(T)} found on asset for pool: {poolData.Name}");
                return;
            }

            // Create a parent GameObject for the pool if one doesn't exist
            var poolParent = new GameObject(poolData.Name).transform;
            poolParent.SetParent(parent != null ? parent : transform);

            var poolHandler = new PoolHandler<T>(component, poolParent);
            _poolHandlers.Add(poolData.Name, poolHandler);
        }

        public T Get<T>(IPoolData poolData) where T : Component
        {
            if (_poolHandlers.TryGetValue(poolData.Name, out var handler) == false)
            {
                Debug.LogWarning($"Pool with name {poolData.Name} not found!");
                return null;
            }

            var poolHandler = handler as PoolHandler<T>;
            var component = poolHandler.Get();

            if (component.TryGetComponent<PoolObject>(out var poolObject))
                poolObject.PoolData = poolData;

            return component;
        }

        public void Release<T>(IPoolData poolData, T component) where T : Component
        {
            if (_poolHandlers.TryGetValue(poolData.Name, out var handler))
            {
                var poolHandler = handler as PoolHandler<T>;
                poolHandler?.Release(component);
            }
            else
            {
                Debug.LogWarning($"Pool with name {poolData.Name} not found!");
            }
        }

        private void OnDestroy()
        {
            foreach (var handler in _poolHandlers.Values)
            {
                (handler as IDisposable)?.Dispose();
            }

            _poolHandlers.Clear();
        }
    }
}
