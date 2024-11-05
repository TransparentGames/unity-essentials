namespace TransparentGames.Essentials.Combat
{
    public interface IHurtboxComponent
    {
        public HitResult OnHit(HitResult hitResult, HitInfo hitInfo);
    }
}