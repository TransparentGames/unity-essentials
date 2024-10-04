using System;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    public abstract class ItemRestriction : ScriptableObject
    {
        public abstract bool IsSatisfiedBy(InventoryItem item);
    }
}