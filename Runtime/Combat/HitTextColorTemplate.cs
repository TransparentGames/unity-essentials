using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    [CreateAssetMenu(fileName = "Hit Text Color Template", menuName = "Transparent Games/Combat/Hit Text Color", order = 0)]
    public class HitTextColorTemplate : ScriptableObject
    {
        public Color normalHitColor;
        public Color criticalHitColor;
    }
}