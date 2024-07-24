using UnityEngine;
using TransparentGames.Essentials.Stats;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Combat;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TransparentGames.Essentials.Abilities
{
    public class Caster : MonoBehaviour, IStatsRequired, IComponent
    {
        public event Action Ready;
        public GameObject Owner { get; set; }
        public Animator Animator => Owner.GetComponentInChildren<Animator>();
        public bool IsBusy => _ability != null && _ability.CanCancel == false;

        [SerializeField] private AbilityTemplate abilityTemplate;

        private Ability _ability = null;

        private StatsHolder _statsHolder;

        private void OnDisable()
        {
            if (_ability != null)
            {
                Abort();
            }
        }

        public bool CanCast()
        {
            if (abilityTemplate == null)
                return false;

            if (abilityTemplate.CanUse(this) == false)
                return false;

            return _ability == null || _ability.CanCancel;
        }

        public void Cast(GameObject target = null)
        {
            if (_ability)
                Abort();

            _ability = Instantiate(abilityTemplate.abilityPrefab, Owner.transform.position, Owner.transform.rotation);

            var hitInfo = abilityTemplate.Calculate(_statsHolder.Stats);
            hitInfo.source = Owner;
            hitInfo.level = _statsHolder.Level;

            _ability.HitInfo = hitInfo;
            _ability.LayerMask = abilityTemplate.layerMask;
            _ability.HitResultsEvent += OnHitResults;
            _ability.Finished += OnAbilityFinished;

            _ability.Use(this);
        }

        public void OnStatsChanged(StatsHolder statsHolder)
        {
            _statsHolder = statsHolder;
        }

        private void OnHitResults(List<HitResult> hitResults)
        {
            foreach (HitResult hitResult in hitResults)
            {

            }
        }

        private void OnAbilityFinished()
        {
            _ability.Finished -= OnAbilityFinished;
            Abort();
            Ready?.Invoke();
        }

        private void Abort()
        {
            Destroy(_ability.gameObject);
            _ability = null;
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
                caster.Cast();
        }
    }
#endif
}