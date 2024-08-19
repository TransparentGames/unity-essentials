using TransparentGames.Essentials.Abilities;

public interface IAbilityComponent
{
    public void Initialize(Caster caster);
    public bool CanUse(Caster caster);
    public void Use(Caster caster);
}