
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransparentGames.Essentials.Data;
using TransparentGames.Essentials.Shop;
using UnityEditor;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [Serializable]
    public class ItemCollection
    {
        public event Action<InventoryItem, bool> Changed;

        public int SlotAmount => slotAmount;
        public string Category => category;
        public IReadOnlyDictionary<int, InventoryItem> Items => _items;

        [SerializeField] private int slotAmount;
        [SerializeField] private string name;
        [SerializeField] private string category;
        [SerializeField] private List<ItemRestriction> restrictions;
        [SerializeField] private List<ItemSlot> itemSlots;

        private readonly Dictionary<int, InventoryItem> _items = new();
        private readonly Dictionary<int, ItemSlot> _itemSlots = new();
        [SerializeField, HideInInspector] private ItemUser _itemUser;

        public bool IsItemPermitted(InventoryItem item)
        {
            var isPermitted = restrictions.Count == 0;
            foreach (var restriction in restrictions)
            {
                if (restriction.IsSatisfiedBy(item, this))
                {
                    return true;
                }
            }

            return isPermitted;
        }

        public bool CanAddItem(InventoryItem item)
        {
            if (IsItemPermitted(item) == false)
            {
                Debug.LogWarning("Item is not permitted");
                return false;
            }

            return IsItemPermitted(item);
        }

        public bool TryAddItem(InventoryItem item, int count = 1)
        {
            if (CanAddItem(item) == false)
                return false;

            for (int i = 0; i < slotAmount; i++)
            {
                if (_items.ContainsKey(i) == false)
                {
                    CreateNewItem(item, i);
                    return true;
                }

                if (UpdateItem(item, i))
                    return true;
            }

            return false;
        }

        public bool CanRemoveItem(InventoryItem item)
        {
            foreach (var key in _items.Keys)
            {
                if (_items[key].ItemInstance.ItemInstanceId == item.ItemInstance.ItemInstanceId)
                {
                    return true;
                }
            }
            return false;
        }

        #region ItemSlotCollection

        /// <summary>
        /// Add item to the item slot collection
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <param name="slotIndex"></param>
        public InventoryItem AddItem(InventoryItem inventoryItem, int slotIndex)
        {
            if (CanAddItem(inventoryItem) == false)
            {
                return null;
            }

            if (_items.ContainsKey(slotIndex))
            {
                OverrideItem(inventoryItem, slotIndex);
                return inventoryItem;
            }

            CreateNewItem(inventoryItem, slotIndex);
            return inventoryItem;
        }


        public InventoryItem RemoveItem(int slotIndex, int amountToRemove = 1)
        {
            if (_items.TryGetValue(slotIndex, out var item) == false)
            {
                return null;
            }

            if (item.ItemInstance.RemainingUses <= amountToRemove)
            {
                _items.Remove(slotIndex);
                Changed?.Invoke(item, false);
                return item;
            }

            item.ItemInstance.RemainingUses -= amountToRemove;
            Changed?.Invoke(item, true);
            return item;
        }

        public InventoryItem GetItemAtSlot(int slotIndex)
        {
            if (_items.TryGetValue(slotIndex, out var item))
            {
                return item;
            }

            return null;
        }

        public int GetItemSlotIndex(InventoryItem inventoryItem)
        {
            foreach (var key in _items.Keys)
            {
                if (_items[key].ItemInstance.ItemInstanceId == inventoryItem.ItemInstance.ItemInstanceId)
                {
                    return key;
                }
            }
            return -1;
        }

        public int GetTargetSlotIndex(InventoryItem inventoryItem)
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (itemSlots[i].itemCategory == inventoryItem.ItemTemplate.itemClass)
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        private void CreateNewItem(InventoryItem item, int index)
        {
            item.ItemInfo = new ItemInfo
            {
                itemCollection = this,
                index = index,
                itemUser = _itemUser
            };
            _items.Add(index, item);
            Changed?.Invoke(item, true);
        }

        private void OverrideItem(InventoryItem item, int index)
        {
            item.ItemInfo = new ItemInfo
            {
                itemCollection = this,
                index = index,
                itemUser = _itemUser
            };
            _items[index] = item;
            Changed?.Invoke(item, true);
        }

        private bool UpdateItem(InventoryItem item, int index)
        {
            if (_items[index].ItemTemplate.IsUnique)
                return false;

            if (_items[index].ItemInstance.ItemId != item.ItemInstance.ItemId)
                return false;

            _items[index].ItemInstance.RemainingUses += item.ItemInstance.RemainingUses;
            Changed?.Invoke(item, true);
            return true;
        }

        public InventoryItem GetItem(int index)
        {
            if (!_items.ContainsKey(index))
            {
                return null;
            }

            return _items[index];
        }

        public void RemoveItem(InventoryItem item)
        {
            foreach (var key in _items.Keys)
            {
                if (_items[key].ItemInstance.ItemInstanceId == item.ItemInstance.ItemInstanceId)
                {
                    _items.Remove(key);
                    Changed?.Invoke(item, false);
                    return;
                }
            }
        }

        public void EditorSetItemUser(ItemUser itemUser)
        {
            _itemUser = itemUser;
        }
    }
}