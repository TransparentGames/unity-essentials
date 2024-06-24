using System;
using System.Collections.Generic;
using UnityEngine;

namespace TransparentGames.Stats
{
    public class StatsHolder : MonoBehaviour
    {
        [SerializeField] private BaseStats baseStats;

        private IStatsRequired[] _statsRequired;
        private IStatUpdater[] _statUpdaters;
        private List<Stat> _stats;

        private void Awake()
        {
            _statsRequired = GetComponentsInChildren<IStatsRequired>();
            _statUpdaters = GetComponentsInChildren<IStatUpdater>();
        }

        private void OnEnable()
        {
            _stats = baseStats.stats;

            foreach (var statUpdater in _statUpdaters)
                statUpdater.StatChanged += OnStatChanged;

            foreach (var statsRequired in _statsRequired)
                statsRequired.OnStatsChanged(_stats);
        }

        private void OnDisable()
        {
            foreach (var statUpdater in _statUpdaters)
            {
                statUpdater.StatChanged -= OnStatChanged;
            }
        }

        private void OnStatChanged(List<Stat> list)
        {
            _stats = baseStats.stats;

            foreach (var statsRequired in _statsRequired)
                statsRequired.OnStatsChanged(_stats);
        }
    }
}