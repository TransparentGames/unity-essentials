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
            var targetCollection = inventory.GetItemCollection(collectionName);
            if (targetCollection == null)
                return;

            InventorySystemManager.MoveToCollectionItemAction(inventoryItem, targetCollection);
        }
    }
}