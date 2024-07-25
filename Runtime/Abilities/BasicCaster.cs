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
    public class BasicCaster : Caster, IStatsRequired
    {
        public override Animator Animator => Owner.GetComponentInChildren<Animator>();
        public override bool IsReady => _ability == null || _ability.CanCancel;

        [SerializeField] protected AbilityTemplate abilityTemplate;

        protected Ability _ability = null;

        private StatsHolder _statsHolder;

        private void OnDisable()
        {
            if (_ability != null)
                _ability.Cancel();
        }

        public override bool CanCast()
        {
            if (abilityTemplate == null)
                return false;

            if (abilityTemplate.CanUse(this) == false)
                return false;

            return IsReady;
        }

        public override void Cast(GameObject target = null)
        {
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

        public override void Cancel()
        {
            if (_ability != null)
                _ability.Cancel();
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

        protected virtual void OnAbilityFinished()
        {
            _ability = null;
            OnReady();
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