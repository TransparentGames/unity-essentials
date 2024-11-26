using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TransparentGames.Essentials.Pools
{
    [Serializable]
    public class PoolDataProperty : IPoolData
    {
        public string Name => assetReference.ToString();
        public AssetReference Reference => assetReference;

        [SerializeField] private AssetReference assetReference;

        public void Release()
        {

        }
    }
}