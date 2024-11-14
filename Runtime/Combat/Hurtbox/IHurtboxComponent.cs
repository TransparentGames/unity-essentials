namespace TransparentGames.Essentials.Combat
{
    /// <summary>
    /// Enum representing the different phases of damage processing in a Hurtbox.
    /// </summary>
    public enum DamagePhase
    {
        /// <summary>
        /// Phase for initial calculations before applying damage.
        /// This phase can include calculations such as knockback and other preliminary effects.
        /// </summary>
        PreCalculation,

        /// <summary>
        /// Phase for calculating the actual damage dealt to the target.
        /// This phase involves determining the base damage value based on various factors.
        /// </summary>
        DamageCalculation,

        /// <summary>
        /// Phase for applying multipliers and other modifications after the base damage has been calculated.
        /// This phase can include applying damage multipliers, resistances, and other modifiers.
        /// </summary>
        PostCalculation,

        /// <summary>
        /// Phase for final adjustments and applying the damage to the target.
        /// This phase involves finalizing the damage value and applying it to the target's health or other attributes.
        /// </summary>
        Finalization
    }

    public interface IHurtboxComponent
    {
        public DamagePhase Phase { get; }
        public bool HandleHit(ref HitInfo hitInfo);
    }
}