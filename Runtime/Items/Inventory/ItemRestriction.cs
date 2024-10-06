using System;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    public abstract class ItemRestriction : ScriptableObject
    {
        public bool IsSatisfiedBy(InventoryItem inventoryItem, ItemCollection itemCollection)
        {
            return IsSatisfiedByInternal(inventoryItem, itemCollection);
        }

        protected abstract bool IsSatisfiedByInternal(InventoryItem inventoryItem, ItemCollection itemCollection);
    }
}