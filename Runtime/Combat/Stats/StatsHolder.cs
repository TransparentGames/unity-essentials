using System;
using System.Collections.Generic;
using UnityEngine;

namespace TransparentGames.Essentials.Stats
{
    public class StatsHolder : MonoBehaviour, ILevelable
    {
        public event Action StatsChanged;
        public event Action<int> LevelChanged;

        public Dictionary<string, Stat> Stats => _currentStats;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                OnStatChanged();
                LevelChanged?.Invoke(_level);
            }
        }

        [SerializeField] private BaseStats baseStats;

        private IStatsRequired[] _statsRequired;
        private IStatUpdater[] _statUpdaters;
        private Dictionary<string, Stat> _currentStats = new();
        private Dictionary<string, Stat> _baseStats = new();
        private int _level = 1;

        private void Awake()
        {
            _statsRequired = GetComponentsInChildren<IStatsRequired>();
            _statUpdaters = GetComponentsInChildren<IStatUpdater>();

            foreach (var statUpdater in _statUpdaters)
                statUpdater.StatChanged += OnStatChanged;
        }

        private void Start()
        {
            OnStatChanged();
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
            _baseStats = baseStats.GetAsDict(_level);
            _currentStats = CalculateStats(_baseStats);
            StatsChanged?.Invoke();

            foreach (var statsRequired in _statsRequired)
                statsRequired.OnStatsChanged(this);
        }

        private Dictionary<string, Stat> CalculateStats(Dictionary<string, Stat> baseStatsDict)
        {
            // Create a deep copy of baseStatsDict to work with
            var finalStatsDict = new Dictionary<string, Stat>();

            foreach (var baseStatEntry in baseStatsDict)
            {
                // Assuming Stat is a class with a copy constructor or a method to create a deep copy
                finalStatsDict.Add(baseStatEntry.Key, new Stat(baseStatEntry.Value.statDefinition, baseStatEntry.Value.Value));
            }

            // Apply updates from statUpdaters.
            foreach (var statUpdater in _statUpdaters)
            {
                var boostedStats = statUpdater.CalculateStats(baseStatsDict);

                foreach (var boostedStat in boostedStats)
                {
                    if (finalStatsDict.ContainsKey(boostedStat.Key))
                        finalStatsDict[boostedStat.Key].Value += boostedStat.Value;
                }
            }
            return finalStatsDict;
        }
    }
}