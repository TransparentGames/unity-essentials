using UnityEngine;
using TransparentGames.Stats;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Combat;
using System;
using TransparentGames.Data;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TransparentGames.Abilities
{
    public class Caster : MonoBehaviour, IStatsRequired, IComponent
    {
        public GameObject Owner { get; set; }
        public Animator Animator => Owner.GetComponentInChildren<Animator>();
        public bool InProgress => _inProgress;

        [SerializeField] private AbilityTemplate abilityTemplate;

        private Ability _ability;
        private bool _inProgress = false;
        private float _damage = 0f;

        public bool CanCast()
        {
            if (abilityTemplate == null)
                return false;

            return abilityTemplate.CanUse(this) && !_inProgress;
        }

        public void Cast(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _ability = Instantiate(abilityTemplate.abilityPrefab, transform.position + direction, Quaternion.Euler(0f, 0f, angle));
            _ability.HitResultsEvent += OnHitResults;
            _ability.Owner = Owner;
            _ability.Damage = _damage;
            _ability.LayerMask = abilityTemplate.layerMask;
            _ability.Use(this);
            _inProgress = true;
            _ability.Finished += OnAbilityFinished;
        }

        public void OnStatsChanged(List<Stat> stats)
        {
            _damage = abilityTemplate.Calculate(stats);
        }

        private void OnHitResults(List<HitResult> hitResults)
        {
            foreach (HitResult hitResult in hitResults)
            {

            }
        }

        private void OnAbilityFinished()
        {
            _ability.HitResultsEvent -= OnHitResults;
            _ability.Finished -= OnAbilityFinished;
            _inProgress = false;
            Destroy(_ability.gameObject);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Caster), true)]
    public class CasterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);

            if (target is not Caster caster)
                return;

            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

            if (GUILayout.Button("Cast"))
                caster.Cast(caster.transform.forward);
        }
    }
#endif
}