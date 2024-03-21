
using System;
using TransparentGames.Combat;
using TransparentGames.Essentials.Combat;

namespace TransparentGames.Combat
{
    public interface IHitbox
    {
        public event Action<IHurtbox> Hit;
        public void StartCheckingCollision();
        public void StopCheckingCollision();
    }
}