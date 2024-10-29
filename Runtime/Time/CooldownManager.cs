using System;
using System.Collections;
using System.Collections.Generic;
using TransparentGames.Essentials;
using TransparentGames.Essentials.Singletons;
using TransparentGames.Essentials.Time;
using UnityEngine;

public class Cooldown : Countdown
{
    public Entity Owner { get; set; }
    public string ID { get; set; }

    public Cooldown(Entity owner, string id, float time) : base(time)
    {
        Owner = owner;
        ID = id;
    }
}

public class CooldownManager : MonoSingleton<CooldownManager>
{
    private List<Cooldown> _cooldowns = new();
    public Cooldown StartCooldown(Entity Owner, string id, float cooldown)
    {
        var cooldownCountdown = GetCooldown(Owner, id);
        if (cooldownCountdown != null)
        {
            cooldownCountdown.Stop();
            cooldownCountdown.Reset(cooldown);
        }
        else
        {
            cooldownCountdown = CreateCooldown(Owner, id, cooldown);
        }

        cooldownCountdown.Start();

        return cooldownCountdown;
    }

    public Cooldown GetCooldown(Entity owner, string id)
    {
        var cooldown = _cooldowns.Find(c => c.Owner == owner && c.ID == id);
        return cooldown;
    }

    public Cooldown CreateCooldown(Entity owner, string id, float time)
    {
        var cooldown = new Cooldown(owner, id, time);
        _cooldowns.Add(cooldown);
        return cooldown;
    }

    private void OnCooldownFinished(Countdown countdown)
    {
        var cooldown = (Cooldown)countdown;
        //_cooldowns.Remove(cooldown);
    }

    private void OnDestroy()
    {
        foreach (var cooldown in _cooldowns)
        {
            cooldown.Stop();
        }
    }
}