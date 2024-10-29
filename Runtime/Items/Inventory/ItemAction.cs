using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    public abstract class ItemAction : ScriptableObject
    {
        public bool CanInvoke(InventoryItem inventoryItem, ItemUser itemUser)
        {
            return CanInvokeInternal(inventoryItem, itemUser);
        }

        public void InvokeAction(InventoryItem inventoryItem, ItemUser itemUser)
        {
            if (!CanInvokeInternal(inventoryItem, itemUser))
                return;

            InvokeActionInternal(inventoryItem, itemUser);
        }

        /// <summary>
        /// Can the item action be invoked.
        /// </summary>
        /// <param name="inventoryItem">The inventory item.</param>
        /// <param name="itemUser">The item user (can be null).</param>
        /// <returns>True if it can be invoked.</returns>
        protected virtual bool CanInvokeInternal(InventoryItem inventoryItem, ItemUser itemUser)
        {
            return true;
        }

        /// <summary>
        /// Consume the item.
        /// </summary>
        /// <param name="inventoryItem">The inventory item.</param>
        /// <param name="itemUser">The item user (can be null).</param>
        protected abstract void InvokeActionInternal(InventoryItem inventoryItem, ItemUser itemUser);
    }
}