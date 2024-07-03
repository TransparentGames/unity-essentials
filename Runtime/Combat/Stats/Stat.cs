using System;
using UnityEngine;

namespace TransparentGames.Stats
{
    [Serializable]
    public class Stat
    {
        public StatDefinition statDefinition;
        public float value;

        public Stat(StatDefinition statDefinition, float value)
        {
            this.statDefinition = statDefinition;
            this.value = value;
        }
    }
}