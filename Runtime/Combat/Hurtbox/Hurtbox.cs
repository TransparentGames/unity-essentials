using System;
using System.Collections;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Stats;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hurtbox : ComponentBase, IStatsRequired, IHittable
{
    public Entity Owner => owner;
    public Transform Transform => transform;
    public event Action<HitResult> HitResultEvent;
    public event Action<HitInfo> HitInfoEvent;

    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color collisionOpenColor;


    private ColliderState _state = ColliderState.Open;
    private IHealth _health;
    private float _defense = 0;
    private Collider _collider;

    private void Start()
    {
        _health = owner.GetComponent<IHealth>();
    }

    public HitResult OnHit(HitInfo hitInfo)
    {
        if (_state == ColliderState.Closed)
            return new HitResult();

        if (_health.CurrentHealth <= 0)
            return new HitResult();

        var dmgReduction = _defense / (_defense + (5 * hitInfo.level) + 500);
        var damage = Mathf.CeilToInt(hitInfo.damage * (1 - dmgReduction));

        _health.Add(-damage);
        HitResult hitResult = new()
        {
            damageDealt = (int)damage,
            wasKilled = _health.CurrentHealth <= 0,
            hitObject = owner,
            isCritical = hitInfo.isCritical
        };

        HitResultEvent?.Invoke(hitResult);
        HitInfoEvent?.Invoke(hitInfo);

        return hitResult;
    }

    public void StartInvincibility()
    {
        _state = ColliderState.Closed;
    }

    public void StopInvincibility()
    {
        _state = ColliderState.Open;
    }

    public void OnStatsChanged(StatsHolder statsHolder)
    {
        if (statsHolder.Stats.TryGetValue("Defense", out Stat defenseStat))
        {
            _defense = defenseStat.Value;
        }
    }
    private void OnDrawGizmos()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();
        CheckGizmoColor();
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(_collider.bounds.center - transform.position, _collider.bounds.size);
    }

    private void CheckGizmoColor()
    {
        switch (_state)
        {
            case ColliderState.Closed:
                Gizmos.color = inactiveColor;
                break;
            case ColliderState.Open:
                Gizmos.color = collisionOpenColor;
                break;
        }
    }
}
