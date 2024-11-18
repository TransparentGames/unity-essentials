
using UnityEngine.AddressableAssets;

namespace TransparentGames.Essentials.Pools
{
    public interface IPoolData
    {
        public string Name { get; }
        public AssetReferenceT<PoolObject> Reference { get; }
    }
}
