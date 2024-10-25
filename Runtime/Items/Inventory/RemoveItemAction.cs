using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [CreateAssetMenu(fileName = "Remove Item Action", menuName = "Transparent Games/Items/Actions/Remove Item", order = 0)]
    public class RemoveItemAction : ItemAction
    {
        protected override bool CanInvokeInternal(InventoryItem inventoryItem, ItemUser itemUser)
        {
            return true;
        }

        protected override void InvokeActionInternal(InventoryItem inventoryItem, ItemUser itemUser)
        {
            inventoryItem.ItemInfo.ItemCollection.RemoveItem(inventoryItem);
        }
    }
}