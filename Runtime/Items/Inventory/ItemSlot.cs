using System;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [Serializable]
    public class ItemSlot
    {
        public string name;
        public ItemCategory itemCategory;
        public int sizeLimit;
    }
}