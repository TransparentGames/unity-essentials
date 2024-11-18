using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TransparentGames.Essentials.Pools
{
    [Serializable]
    public class PoolDataProperty : IPoolData
    {
        public string Name => assetReference.ToString();
        public AssetReferenceT<PoolObject> Reference => assetReference;

        [SerializeField] private AssetReferenceT<PoolObject> assetReference;

        public void Release()
        {

        }
    }
}