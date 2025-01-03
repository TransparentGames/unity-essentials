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
        public override Ability CurrentAbility => _ability;
        public override bool IsBusy => _inProgress;

        [SerializeField] private AbilityTemplate _abilityTemplate = null;
        protected Ability _ability = null;
        protected bool _inProgress = false;

        private StatsHolder _statsHolder;

        private void Awake()
        {
            if (_abilityTemplate != null)
                Equip(_abilityTemplate);
        }

        public override bool CanCast()
        {
            if (_ability == null)
                return false;

            return _ability.CanUse(this);
        }

        public override Ability Cast(GameObject target = null)
        {
            _inProgress = true;

            _ability.HitResult += OnHitResult;
            _ability.Finished += OnAbilityFinished;

            _ability.Use(this);

            return _ability;
        }

        public override void Equip(AbilityTemplate abilityTemplate)
        {
            Unequip();
            _ability = Instantiate(abilityTemplate.abilityPrefab);
            _ability.Initialize(this);
            OnEquipped();
        }

        public override void Unequip()
        {
            if (_ability == null)
                return;

            _ability = null;

            OnUnequipped();
        }

        public override void Cancel()
        {
            if (_ability != null)
            {
                _ability.Cancel();
                _ability.Finished -= OnAbilityFinished;
                _inProgress = false;
                OnReady();
            }
        }

        public void OnStatsChanged(StatsHolder statsHolder)
        {
            _statsHolder = statsHolder;
        }

        private void OnHitResult(HitResult hitResult)
        {

        }

        protected virtual void OnAbilityFinished(Ability ability)
        {
            ability.Finished -= OnAbilityFinished;
            _inProgress = false;
            OnReady();
        }
    }
}