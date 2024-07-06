using System.Collections.Generic;
using UnityEngine;

namespace TransparentGames.Stats
{
    [CreateAssetMenu(fileName = "New Base Stats", menuName = "Transparent Games/Stats/Base Stats", order = 0)]
    public class BaseStats : ScriptableObject
    {
        public List<Stat> stats;

        public Dictionary<string, Stat> GetAsDict()
        {
            var statsDict = new Dictionary<string, Stat>();

            foreach (var stat in stats)
            {
                statsDict[stat.statDefinition.statName] = stat;
            }

            return statsDict;
        }
    }
}