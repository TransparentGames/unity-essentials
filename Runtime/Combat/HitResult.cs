using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public struct HitResult
    {
        public int damageDealt;
        public bool wasKilled;
        public Entity hitObject;
        public bool isCritical;
    }
}