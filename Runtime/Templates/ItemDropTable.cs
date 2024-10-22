using System;
using System.Collections.Generic;
using TransparentGames.Essentials.Items;
using UnityEngine;

namespace TransparentGames.Essentials.Enemies
{
    public abstract class ItemDropTable : ScriptableObject
    {
        public abstract List<RewardItem> GetRandomRewardItems();
    }
}