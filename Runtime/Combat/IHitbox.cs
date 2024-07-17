
using System;
using TransparentGames.Essentials.Combat;

namespace TransparentGames.Essentials.Combat
{
    public interface IHitbox
    {
        public event Action<IHurtbox> Hit;
        public void StartCheckingCollision();
        public void StopCheckingCollision();
    }
}