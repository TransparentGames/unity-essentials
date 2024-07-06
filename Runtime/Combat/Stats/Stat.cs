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

        public Stat(string key, float value)
        {
            // TODO get this value from a StatCollection
            //statDefinition = new StatDefinition(key);
            this.value = value;
        }
    }
}