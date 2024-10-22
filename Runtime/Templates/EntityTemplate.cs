using TransparentGames.Essentials;
using TransparentGames.Essentials.Stats;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TransparentGames.Entities
{
    public abstract class EntityTemplate : ScriptableObject
    {
        public string entityId;
        [Space]
        public Sprite icon;
        public AssetReference assetReference;
        public BaseStats baseStats;
    }

}