using System;
using TransparentGames.Essentials.Currency;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [Serializable]
    public class RewardItem
    {
        public string item_id = "CN";
        public string data_type = "currency";
        public int count = 1;

        public RewardItem()
        {
        }

        public RewardItem(string itemId, string dataType)
        {
            item_id = itemId;
            data_type = dataType;
        }
    }

    public static class DataType
    {
        public static readonly string Inventory = "inventory";
        public static readonly string PlayerData = "player_data";
        public static readonly string Currency = "currency";
    }
}