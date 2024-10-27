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

        [SerializeField] private AnimationCurve curve;
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
            curve = stat.curve;
            scalableValue = stat.scalableValue;
            _level = level;
        }
    }
}