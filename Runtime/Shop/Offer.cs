
using TransparentGames.Essentials.Data.Nodes;
using TransparentGames.Essentials.Items;
using UnityEngine;

namespace TransparentGames.Essentials.Shop
{
    [CreateAssetMenu(fileName = "Offer", menuName = "Transparent Games/Shop/Offer", order = 0)]
    public class Offer : ScriptableObject
    {
        public RewardItem rewardItem;
        public Price price;
    }
}