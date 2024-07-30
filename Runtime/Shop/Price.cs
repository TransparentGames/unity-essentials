
using System;
using TransparentGames.Essentials.Data.Nodes;
using TransparentGames.Essentials.Items;
using UnityEngine;

namespace TransparentGames.Essentials.Shop
{
    [Serializable]
    public class Price
    {
        public bool CanAfford => currencyNode.Value >= amount;

        public CurrencyNode currencyNode;
        public int amount;

        public Price(CurrencyNode currencyNode, int amount)
        {
            this.currencyNode = currencyNode;
            this.amount = amount;
        }
    }
}