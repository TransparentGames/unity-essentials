using System;
using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine.Assertions;

namespace TransparentGames.Essentials.UpdateManagement
{
    public class UpdateManager : PersistentMonoSingleton<UpdateManager>
    {
        private List<UpdateEntity> _updateEntities = new();
        private List<UpdateEntity> _fixedUpdateEntities = new();
        private List<UpdateEntity> _lateUpdateEntities = new();

        private List<UpdateEntity> _updateEntitiesToRemove = new();
        private List<UpdateEntity> _fixedUpdateEntitiesToRemove = new();
        private List<UpdateEntity> _lateUpdateEntitiesToRemove = new();

        public static IUpdateEntity StartUpdate(Action onUpdateAction, UpdateType updateType, int interval = 1)
        {
            return Instance.InternalStartUpdate(onUpdateAction, updateType, interval);
        }

        private IUpdateEntity InternalStartUpdate(Action onUpdateAction, UpdateType updateType, int interval = 1)
        {
            var updateEntity = new UpdateEntity(onUpdateAction, interval);
            GetListForUpdateType(updateType).Add(updateEntity);
            return updateEntity;
        }

        private List<UpdateEntity> GetListForUpdateType(UpdateType updateType)
        {
            return updateType switch
            {
                UpdateType.Update => _updateEntities,
                UpdateType.FixedUpdate => _fixedUpdateEntities,
                UpdateType.LateUpdate => _lateUpdateEntities,
                _ => throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null),
            };
        }

        public static void Stop(IUpdateEntity updateEntity)
        {
            if (InstanceExists == false) return;

            Instance.InternalStop(updateEntity);
        }

        private void InternalStop(IUpdateEntity updateEntity)
        {
            var entity = updateEntity as UpdateEntity;
            Assert.IsNotNull(entity);

            if (_updateEntities.Contains(entity))
                _updateEntitiesToRemove.Add(entity);
            else if (_fixedUpdateEntities.Contains(entity))
                _fixedUpdateEntitiesToRemove.Add(entity);
            else if (_lateUpdateEntities.Contains(entity))
                _lateUpdateEntitiesToRemove.Add(entity);
        }

        private void Update()
        {
            SafelyRemoveEntities(_updateEntitiesToRemove, _updateEntities);

            for (int i = 0; i < _updateEntities.Count; i++)
            {
                _updateEntities[i].Tick();
            }
        }

        private void FixedUpdate()
        {
            SafelyRemoveEntities(_fixedUpdateEntitiesToRemove, _fixedUpdateEntities);

            for (int i = 0; i < _fixedUpdateEntities.Count; i++)
            {
                _fixedUpdateEntities[i].Tick();
            }
        }

        private void LateUpdate()
        {
            SafelyRemoveEntities(_lateUpdateEntitiesToRemove, _lateUpdateEntities);

            for (int i = 0; i < _lateUpdateEntities.Count; i++)
            {
                _lateUpdateEntities[i].Tick();
            }
        }

        private void SafelyRemoveEntities(List<UpdateEntity> entities, List<UpdateEntity> listFrom)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                listFrom.Remove(entities[i]);
            }

            entities.Clear();
        }
    }
}