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
        public StatsHolder StatsHolder { get; set; }
        public Animator Animator => Owner.GetComponentInChildren<Animator>();
        public bool IsBusy => _abilityInProgress;

        [SerializeField] private AbilityTemplate abilityTemplate;

        private Ability _ability;
        private bool _abilityInProgress = false;

        public bool CanCast()
        {
            if (abilityTemplate == null)
                return false;

            return abilityTemplate.CanUse(this) && !_abilityInProgress;
        }

        public void Cast(Vector3 position)
        {
            _ability = Instantiate(abilityTemplate.abilityPrefab, position, Owner.transform.rotation);
            _ability.HitResultsEvent += OnHitResults;
            _ability.Owner = Owner;
            _ability.Damage = abilityTemplate.Calculate(StatsHolder.Stats);
            _ability.LayerMask = abilityTemplate.layerMask;
            _ability.Use(this);
            _abilityInProgress = true;
            _ability.Finished += OnAbilityFinished;
        }


        public void OnStatsChanged()
        {
            // Does not need to update, the ability will be updated when it is cast
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
            _abilityInProgress = false;
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
                caster.Cast(caster.Owner.transform.position);
        }
    }
#endif
}