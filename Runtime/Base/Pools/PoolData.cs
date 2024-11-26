using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TransparentGames.Essentials.Pools
{
    [CreateAssetMenu(fileName = "Pool Data", menuName = "Transparent Games/Data/Pool", order = 0)]
    public class PoolData : ScriptableObject, IPoolData
    {
        public AssetReference Reference { get => assetReference; set => assetReference = value; }

        public string Name => name;

        [SerializeField] private AssetReference assetReference;

        public void Release()
        {

        }
    }
}
