using TransparentGames.Essentials.Abilities;

public interface IAbilityComponent
{
    public bool CanUse(Caster caster);
    public void Use(Caster caster);
}