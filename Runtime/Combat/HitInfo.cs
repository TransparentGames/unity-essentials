using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public struct HitInfo
    {
        public float damage;
        public GameObject source;
        public int level;
        public bool isCritical;
    }
}