using TransparentGames.Essentials.Pools;
using TransparentGames.Essentials.Stats;
using UnityEngine;

namespace TransparentGames.Entities
{
    public abstract class EntityTemplate : ScriptableObject
    {
        public string entityId;
        [Space]
        public Sprite icon;
        public PoolDataProperty poolData;
        public BaseStats baseStats;
    }

}