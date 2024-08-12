using UnityEngine;
using TransparentGames.Essentials.Stats;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Combat;
using System;

namespace TransparentGames.Essentials.Abilities
{
    public class BasicCaster : Caster, IStatsRequired
    {
        public override Animator Animator => Owner.GetComponentInChildren<Animator>();
        public override bool IsBusy => _inProgress;
        public override bool CanCancel => !IsBusy || _ability.CanCancel;
        public override bool CanHardCancel => CanCancel || _ability.CanHardCancel;

        [SerializeField] private AbilityTemplate _abilityTemplate = null;
        protected Ability _ability = null;
        protected bool _inProgress = false;

        private StatsHolder _statsHolder;

        private void Start()
        {
            if (_abilityTemplate != null)
                Equip(_abilityTemplate);
        }

        private void OnDisable()
        {
            if (_ability != null)
                _ability.Cancel();
        }

        public override bool CanCast()
        {
            if (_ability == null)
                return false;

            return _ability.CanUse(this);
        }

        public override void Cast(GameObject target = null)
        {
            _inProgress = true;

            _ability.HitResultsEvent += OnHitResults;
            _ability.Finished += OnAbilityFinished;

            _ability.Use(this);
        }

        public override void Equip(AbilityTemplate abilityTemplate)
        {
            Unequip();
            _ability = Instantiate(abilityTemplate.abilityPrefab, Owner.transform.position, Owner.transform.rotation);
        }

        public override void Unequip()
        {
            if (_ability == null)
                return;

            Destroy(_ability.gameObject);

            _ability = null;
        }

        public override void Cancel()
        {
            if (_ability != null)
            {
                _ability.Cancel();
            }
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
            _inProgress = false;
            OnReady();
        }
    }
}