using System;
using TransparentGames.Essentials.Currency;
using TransparentGames.Essentials.Data.Nodes;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [Serializable]
    public class RewardItem
    {
        public string ItemId => string.IsNullOrEmpty(_itemId) ? itemTemplate.itemId : _itemId;
        public int count = 1;
        [SerializeField] private ItemTemplate itemTemplate;

        private string _itemId;

        public RewardItem()
        {
        }

        public RewardItem(string itemId)
        {
            _itemId = itemId;
        }
    }
}