using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [CreateAssetMenu(fileName = "Equip Item Action", menuName = "Transparent Games/Items/Actions/Equip Item", order = 0)]
    public class EquipItemAction : ItemAction
    {
        [SerializeField] private string collectionName;

        protected override bool CanInvokeInternal(InventoryItem inventoryItem, ItemUser itemUser)
        {
            return true;
        }

        protected override void InvokeActionInternal(InventoryItem inventoryItem, ItemUser itemUser)
        {
            var inventory = itemUser.GetComponent<Inventory>();

            if (inventory.GetItemCollection(inventoryItem.ItemInfo.itemCollectionName).Purpose == ItemCollectionPurpose.Equipped)
            {
                var defaultCollection = inventory.GetDefaultItemCollection(inventoryItem);
                InventorySystemManager.MoveToCollectionItemAction(inventoryItem, defaultCollection);
                return;
            }

            var targetCollection = inventory.GetItemCollection(collectionName);
            if (targetCollection == null)
                return;

            InventorySystemManager.MoveToSlotCollectionItemAction(inventoryItem, targetCollection);
        }
    }
}