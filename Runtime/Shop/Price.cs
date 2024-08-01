
using System;
using TransparentGames.Essentials.Data.Nodes;
using TransparentGames.Essentials.Items;
using UnityEngine;

namespace TransparentGames.Essentials.Shop
{
    [Serializable]
    public class Price
    {
        public ItemTemplate itemTemplate;
        public int amount;

        public Price(ItemTemplate itemTemplate, int amount)
        {
            this.itemTemplate = itemTemplate;
            this.amount = amount;
        }
    }
}