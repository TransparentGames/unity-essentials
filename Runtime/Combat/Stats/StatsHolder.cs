using System;
using System.Collections.Generic;
using UnityEngine;

namespace TransparentGames.Stats
{
    public class StatsHolder : MonoBehaviour
    {
        public List<Stat> Stats => CalculateStats();
        public List<Stat> BaseStats => baseStats.stats;

        [SerializeField] private BaseStats baseStats;

        private IStatsRequired[] _statsRequired;
        private IStatUpdater[] _statUpdaters;
        //private List<Stat> _stats;

        private void Awake()
        {
            _statsRequired = GetComponentsInChildren<IStatsRequired>();
            _statUpdaters = GetComponentsInChildren<IStatUpdater>();
        }

        private void OnEnable()
        {
            foreach (var statUpdater in _statUpdaters)
                statUpdater.StatChanged += OnStatChanged;

            foreach (var statsRequired in _statsRequired)
            {
                statsRequired.StatsHolder = this;
                statsRequired.OnStatsChanged();
            }
        }

        private void OnDisable()
        {
            foreach (var statUpdater in _statUpdaters)
            {
                statUpdater.StatChanged -= OnStatChanged;
            }
        }

        private void OnStatChanged()
        {
            foreach (var statsRequired in _statsRequired)
                statsRequired.OnStatsChanged();
        }

        private List<Stat> CalculateStats()
        {
            var finalStats = baseStats.stats;

            foreach (var statUpdater in _statUpdaters)
                finalStats = statUpdater.CalculateStats(baseStats.stats);

            return finalStats;
        }
    }
}