using System;
using System.Collections.Generic;
using UnityEngine;


namespace TransparentGames.Essentials.Stats
{
    [Serializable]
    public class ScalableStat : Stat
    {
        public AnimationCurve curve;
        public float scalableValue;

        public ScalableStat(StatDefinition statDefinition, float value) : base(statDefinition, value)
        {

        }

        public float Calculate(int level)
        {
            var finalValue = Value + scalableValue * curve.Evaluate((float)level / 100);
            return finalValue;
        }
    }
}