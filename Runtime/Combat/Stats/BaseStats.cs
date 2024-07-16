using System;
using System.Collections.Generic;
using UnityEngine;


namespace TransparentGames.Essentials.Stats
{
    [CreateAssetMenu(fileName = "New Base Stats", menuName = "Transparent Games/Stats/Base Stats", order = 0)]
    public class BaseStats : ScriptableObject
    {
        public List<ScalableStat> stats;

        public Dictionary<string, Stat> GetAsDict(int level)
        {
            var statsDict = new Dictionary<string, Stat>();

            foreach (var stat in stats)
            {
                statsDict[stat.statDefinition.statName] = new Stat(stat.statDefinition, stat.Calculate(level));
            }

            return statsDict;
        }
    }
}