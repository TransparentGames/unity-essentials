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
        private Dictionary<string, Stat> _baseStats = new();

        private void Awake()
        {
            _statsRequired = GetComponentsInChildren<IStatsRequired>();
            _statUpdaters = GetComponentsInChildren<IStatUpdater>();

            _baseStats = baseStats.GetAsDict();
            _currentStats = CalculateStats(_baseStats);

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
            _currentStats = CalculateStats(_baseStats);

            foreach (var statsRequired in _statsRequired)
                statsRequired.OnStatsChanged();
        }

        private Dictionary<string, Stat> CalculateStats(Dictionary<string, Stat> baseStatsDict)
        {
            // Create a deep copy of baseStatsDict to work with
            var finalStatsDict = new Dictionary<string, Stat>();

            foreach (var baseStatEntry in baseStatsDict)
            {
                // Assuming Stat is a class with a copy constructor or a method to create a deep copy
                finalStatsDict.Add(baseStatEntry.Key, new Stat(baseStatEntry.Value.statDefinition, baseStatEntry.Value.value));
            }

            // Apply updates from statUpdaters.
            foreach (var statUpdater in _statUpdaters)
            {
                var boostedStats = statUpdater.CalculateStats(baseStatsDict);

                foreach (var boostedStat in boostedStats)
                {
                    if (finalStatsDict.ContainsKey(boostedStat.Key))
                        finalStatsDict[boostedStat.Key].value += boostedStat.Value;
                    else
                        finalStatsDict.Add(boostedStat.Key, new Stat(boostedStat.Key, boostedStat.Value));
                }
            }
            return finalStatsDict;
        }
    }
}