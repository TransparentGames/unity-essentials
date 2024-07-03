using System;
using System.Collections.Generic;
using UnityEngine;

namespace TransparentGames.Stats
{
    public class StatsHolder : MonoBehaviour
    {
        public Dictionary<string, Stat> Stats => _currentStats;

        [SerializeField] private BaseStats baseStats;

        private IStatsRequired[] _statsRequired;
        private IStatUpdater[] _statUpdaters;
        private Dictionary<string, Stat> _currentStats = new();

        private void Awake()
        {
            _statsRequired = GetComponentsInChildren<IStatsRequired>();
            _statUpdaters = GetComponentsInChildren<IStatUpdater>();

            _currentStats = CalculateStats();

            foreach (var statUpdater in _statUpdaters)
                statUpdater.StatChanged += OnStatChanged;

            foreach (var statsRequired in _statsRequired)
            {
                statsRequired.StatsHolder = this;
                statsRequired.OnStatsChanged();
            }
        }

        private void OnDestroy()
        {
            foreach (var statUpdater in _statUpdaters)
            {
                statUpdater.StatChanged -= OnStatChanged;
            }
        }

        private void OnStatChanged()
        {
            _currentStats = CalculateStats();

            foreach (var statsRequired in _statsRequired)
                statsRequired.OnStatsChanged();
        }

        private Dictionary<string, Stat> CalculateStats()
        {
            var finalStatsDict = new Dictionary<string, Stat>();

            // Initialize finalStatsDict with base stats.
            foreach (var baseStat in baseStats.stats)
            {
                finalStatsDict[baseStat.statDefinition.statName] = new Stat(baseStat.statDefinition, baseStat.value);
            }

            // Apply updates from statUpdaters.
            foreach (var statUpdater in _statUpdaters)
            {
                foreach (var stat in statUpdater.CalculateStats(baseStats.stats))
                {
                    if (finalStatsDict.TryGetValue(stat.statDefinition.statName, out var existingStat))
                    {
                        existingStat.value += stat.value;
                    }
                    else
                    {
                        // This case should not happen if all stats are initialized correctly,
                        // but it's here to ensure the method is robust to unexpected inputs.
                        finalStatsDict[stat.statDefinition.statName] = new Stat(stat.statDefinition, stat.value);
                    }
                }
            }

            return finalStatsDict;
        }
    }
}