using System;
using UnityEngine;

namespace TransparentGames.Essentials.Stats
{
    [Serializable]
    public class ScalableStat : Stat
    {
        public override float Value
        {
            get { return base.Value + scalableValue * curve.Evaluate((float)_level / 100); }
            set { base.Value = value; }
        }

        public AnimationCurve curve;
        public float scalableValue;

        private int _level;

        public ScalableStat(StatDefinition statDefinition, float value, int level)
            : base(statDefinition, value)
        {
            _level = level;
        }

        public ScalableStat(ScalableStat stat, int level)
            : base(stat)
        {
            _level = level;
            curve = stat.curve ?? throw new ArgumentNullException(nameof(stat.curve));
            scalableValue = stat.scalableValue;
        }
    }
}