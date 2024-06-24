using System.Collections.Generic;
using UnityEngine;

namespace TransparentGames.Stats
{
    [CreateAssetMenu(fileName = "New Base Stats", menuName = "Transparent Games/Stats/Base Stats", order = 0)]
    public class BaseStats : ScriptableObject
    {
        public List<Stat> stats;
    }
}