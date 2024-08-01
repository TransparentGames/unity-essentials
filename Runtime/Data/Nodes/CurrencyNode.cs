using System;
using TransparentGames.Essentials.Currency;
using UnityEngine;

namespace TransparentGames.Essentials.Data.Nodes
{
    [CreateAssetMenu(fileName = "Currency Node", menuName = "Transparent Games/Data/Currency Node", order = 0)]
    public class CurrencyNode : BaseNode<int>
    {
        public string Key => key;

        [SerializeField] protected string key;
        [SerializeField] protected int defaultValue;
        [SerializeField] private Sprite icon;

        public Sprite Icon => icon;
        public override string DisplayValue => Value.ToString();
        public override int Value
        {
            get
            {
                return CurrencyManager.Instance.Get(key, defaultValue).Value;
            }
            set
            {
                CurrencyManager.Instance.Get(key, defaultValue).Value = value;
            }
        }

        public override void AddListener(Action callback)
        {
            CurrencyManager.Instance.Get(key, defaultValue).Changed += callback;
        }

        public override void RemoveListener(Action callback)
        {
            if (CurrencyManager.InstanceExists)
                CurrencyManager.Instance.Get(key, defaultValue).Changed -= callback;
        }
    }
}