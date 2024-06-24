using TransparentGames.Essentials;
using TransparentGames.Stats;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TransparentGames.Entities
{
    public abstract class EntityTemplate : ScriptableObject
    {
        public string itemId;
        [Space]
        public Sprite icon;
        public AssetReference assetReference;
        public BaseStats baseStats;
    }
}