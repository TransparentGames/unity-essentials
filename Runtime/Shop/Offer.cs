
using TransparentGames.Essentials.Data.Nodes;
using TransparentGames.Essentials.Items;
using UnityEngine;

namespace TransparentGames.Essentials.Shop
{
    [CreateAssetMenu(fileName = "Offer", menuName = "Transparent Games/Shop/Offer", order = 0)]
    public class Offer : ScriptableObject
    {
        public bool CanAfford => currencyNode.Value >= price;

        public RewardItem rewardItem;
        public CurrencyNode currencyNode;
        public int price;
    }
}