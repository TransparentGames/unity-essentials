using UnityEngine;

namespace TransparentGames.Essentials
{
    public interface IComponent
    {
        public GameObject Owner { get; set; }
    }
}
