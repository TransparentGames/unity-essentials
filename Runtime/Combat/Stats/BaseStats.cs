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
                var calculatedStat = new ScalableStat(stat, level);
                statsDict[stat.statDefinition.statName] = calculatedStat;
            }

            return statsDict;
        }
    }
}