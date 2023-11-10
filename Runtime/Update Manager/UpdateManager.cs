using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UpdateManager : MonoSingleton<UpdateManager>
{
    private List<UpdateEntity> _updateEntities = new();
    private List<UpdateEntity> _fixedUpdateEntities = new();
    private List<UpdateEntity> _lateUpdateEntities = new();


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
        switch (updateType)
        {
            case UpdateType.Update:
                return _updateEntities;

            case UpdateType.FixedUpdate:
                return _fixedUpdateEntities;

            case UpdateType.LateUpdate:
                return _lateUpdateEntities;

            default:
                throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
        }
    }

    public static void Stop(IUpdateEntity updateEntity)
    {
        if (IsInitialized == false) return;

        Instance.InternalStop(updateEntity);
    }

    private void InternalStop(IUpdateEntity updateEntity)
    {
        var entity = updateEntity as UpdateEntity;
        Assert.IsNotNull(entity);

        if (_updateEntities.Contains(entity))
            _updateEntities.Remove(entity);
        else if (_fixedUpdateEntities.Contains(entity))
            _fixedUpdateEntities.Remove(entity);
        else if (_lateUpdateEntities.Contains(entity))
            _lateUpdateEntities.Remove(entity);
    }

    private void Update()
    {
        for (int i = 0; i < _updateEntities.Count; i++)
        {
            _updateEntities[i].Tick();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _fixedUpdateEntities.Count; i++)
        {
            _fixedUpdateEntities[i].Tick();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _lateUpdateEntities.Count; i++)
        {
            _lateUpdateEntities[i].Tick();
        }
    }
}