using System;
using TransparentGames.Essentials;
using UnityEngine;

namespace TransparentGames.Data
{
    [CreateAssetMenu(fileName = "Session Node", menuName = "Transparent Games/Data/Session Node", order = 0)]
    public class SessionNode : BaseNode<int>
    {
        [SerializeField] private string key;
        [SerializeField] private int defaultValue;
        public override string DisplayValue => Value.ToString();

        public override int Value
        {
            get
            {
                return CachedDataManager.Instance.GetProperty(key, defaultValue).Value;
            }
            set
            {
                CachedDataManager.Instance.GetProperty(key, defaultValue).Value = value;
            }
        }

        public override void AddListener(Action callback)
        {
            CachedDataManager.Instance.GetProperty(key, defaultValue).Changed += callback;
        }

        public override void RemoveListener(Action callback)
        {
            CachedDataManager.Instance.GetProperty(key, defaultValue).Changed -= callback;
        }
    }
}