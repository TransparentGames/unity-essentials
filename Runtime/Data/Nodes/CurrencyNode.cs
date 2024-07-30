using System;
using TransparentGames.Essentials.Currency;
using UnityEngine;

namespace TransparentGames.Essentials.Data.Nodes
{
    [CreateAssetMenu(fileName = "Currency Node", menuName = "Transparent Games/Data/Currency Node", order = 0)]
    public class CurrencyNode : BaseNode<int>
    {
        [SerializeField] protected int defaultValue;

        public Sprite Icon => icon;
        public override string DisplayValue => Value.ToString();
        public override int Value
        {
            get
            {
                return CurrencyManager.Instance.Get(itemId, defaultValue).Value;
            }
            set
            {
                CurrencyManager.Instance.Get(itemId, defaultValue).Value = value;
            }
        }

        public override void AddListener(Action callback)
        {
            CurrencyManager.Instance.Get(itemId, defaultValue).Changed += callback;
        }

        public override void RemoveListener(Action callback)
        {
            if (CurrencyManager.InstanceExists)
                CurrencyManager.Instance.Get(itemId, defaultValue).Changed -= callback;
        }
    }
}