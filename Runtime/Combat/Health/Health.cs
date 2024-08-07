using System;
using UnityEngine;
using TransparentGames.Essentials.Stats;
using System.Collections.Generic;
using TransparentGames.Essentials.Combat;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TransparentGames.Essentials.Combat
{
    public class Health : MonoBehaviour, IHealth, IStatsRequired
    {
        public event Action ValueInitialized;
        public event Action<float> ValueChanged;
        public event Action ValueZeroed;
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        private float _maxHealth = 9999f;
        private float _currentHealth = 9999f;

        public virtual void Add(float amount)
        {
            if (_currentHealth + amount > _maxHealth)
                _currentHealth = _maxHealth;
            else
                _currentHealth += amount;
            ValueChanged?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
                ValueZeroed?.Invoke();
        }

        public void OnStatsChanged(StatsHolder statsHolder)
        {
            if (statsHolder.Stats.TryGetValue("Health", out Stat healthStat))
            {
                _maxHealth = healthStat.Value;
                if (_currentHealth > _maxHealth)
                {
                    _currentHealth = _maxHealth;
                    ValueInitialized?.Invoke();
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Health), true)]
    public class HealthEditor : Editor
    {
        private float _healthValue = 10.0f;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);
            GUILayout.Label("Editor", EditorStyles.boldLabel);

            if (target is not Health health)
                return;

            // EditorGUI.BeginDisabledGroup(true);
            // EditorGUILayout.FloatField("Max Health", health.MaxHealth);
            // EditorGUILayout.FloatField("Current Health", health.CurrentHealth);
            // EditorGUI.EndDisabledGroup();

            _healthValue = EditorGUILayout.FloatField("Health", _healthValue);

            if (GUILayout.Button("Add Health"))
            {
                health.Add(_healthValue);
            }
        }
    }
#endif
}