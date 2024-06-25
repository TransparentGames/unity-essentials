using System;
using UnityEngine;
using TransparentGames.Stats;
using System.Collections.Generic;
using TransparentGames.Essentials.Combat;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TransparentGames.Combat
{
    public class Health : MonoBehaviour, IHealth, IStatsRequired
    {
        public StatsHolder StatsHolder { get; set; }

        public event Action<float> ValueChanged;
        public event Action ValueZeroed;
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;


        private float _maxHealth = 999f;
        private float _currentHealth = 999f;

        public virtual void Add(float amount)
        {
            _currentHealth += amount;
            ValueChanged?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
                ValueZeroed?.Invoke();
        }

        public void OnStatsChanged()
        {
            _maxHealth = StatsHolder.Stats.Find(stat => stat.statDefinition.statName == "Health").value;
            _currentHealth = _maxHealth;
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

            GUILayout.Space(20);

            if (target is not Health health)
                return;

            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.FloatField("Max Health", health.MaxHealth);
            EditorGUILayout.FloatField("Current Health", health.CurrentHealth);
            EditorGUI.EndDisabledGroup();

            _healthValue = EditorGUILayout.FloatField("Health", _healthValue);

            if (GUILayout.Button("Add Health"))
            {
                health.Add(_healthValue);
            }
        }
    }
#endif
}