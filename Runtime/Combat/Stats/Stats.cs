using System;
using System.Collections.Generic;
using UnityEngine;

namespace TransparentGames.Combat
{
    [Serializable]
    public class Stats
    {
        public StatFloat Attack = new("attack", 0);
        public StatFloat Health = new("health", 0);
        public StatFloat Mana = new("mana", 0);
    }
}