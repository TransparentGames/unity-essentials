
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TransparentGames.Essentials.Data;
using TransparentGames.Essentials.Shop;
using UnityEditor;
using UnityEngine;


namespace TransparentGames.Essentials.Items
{
    public enum ItemCollectionPurpose
    {
        None,
        Main,
        Secondary,
        Equipped
    }


    [Serializable]
    public class ItemCollection
    {
        public event Action<InventoryItem, bool> Changed;

        public int SlotAmount => slotAmount;
        public string Name => name;
        public IReadOnlyDictionary<int, InventoryItem> Items => _items;
        public ItemCollectionPurpose Purpose => purpose;

        [SerializeField] private int slotAmount;
        [SerializeField] private string name;
        [SerializeField] private ItemCollectionPurpose purpose;
        [SerializeField] private List<ItemRestriction> restrictions;
        [SerializeField] private List<ItemSlot> itemSlots;

        private readonly Dictionary<int, InventoryItem> _items = new();
        [SerializeField, HideInInspector] private ItemUser _itemUser;

        public bool CanAddItem(InventoryItem item)
        {
            if (IsItemPermitted(item) == false)
            {
                return false;
            }

            return IsItemPermitted(item);
        }

        public bool CanAddItem(InventoryItem item, int slotIndex)
        {
            if (IsItemPermitted(item) == false)
            {
                return false;
            }

            if (IsRestrictedByItemSlot(item, slotIndex) == false)
            {
                return false;
            }

            return true;
        }

        public bool TryAddItem(InventoryItem item)
        {
            if (CanAddItem(item) == false)
                return false;

            if (item.ItemTemplate.IsUnique == false)
            {
                foreach (var key in _items.Keys)
                {
                    if (UpdateItem(item, key))
                        return true;
                }
            }

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

        public InventoryItem AddItem(InventoryItem inventoryItem)
        {
            if (CanAddItem(inventoryItem) == false)
            {
                return null;
            }

            // TODO: Make this item collection part of the flow using only the name and internal function to get collection owner etc
            inventoryItem.ItemInfo.ItemCollection = this;

            _items[inventoryItem.ItemInfo.index] = inventoryItem;
            Changed?.Invoke(inventoryItem, true);
            return inventoryItem;
        }

        public InventoryItem RemoveItem(int slotIndex)
        {
            if (_items.TryGetValue(slotIndex, out var item) == false)
            {
                return null;
            }

            _items.Remove(slotIndex);
            Changed?.Invoke(item, false);
            return item;
        }

        public InventoryItem RemoveItem(int slotIndex, int amountToRemove)
        {
            if (_items.TryGetValue(slotIndex, out var item) == false)
            {
                return null;
            }

            if (item.ItemInstance.RemainingUses <= amountToRemove)
            {
                return RemoveItem(slotIndex);
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

        public int GetTargetSlotIndex(InventoryItem inventoryItem, bool checkAvailability = false)
        {
            int cumulativeIndex = 0;

            for (int i = 0; i < itemSlots.Count; i++)
            {
                ItemSlot itemSlot = itemSlots[i];

                // Check if the inventory item matches this slot's item category
                if (itemSlot.itemCategory.Name == inventoryItem.ItemTemplate.itemCategory.Name)
                {
                    // Loop through each index in the slot's range based on sizeLimit
                    for (int slotIndex = cumulativeIndex; slotIndex < cumulativeIndex + itemSlot.sizeLimit; slotIndex++)
                    {
                        // If checking availability, ensure the slot is free
                        if (checkAvailability)
                        {
                            if (!_items.ContainsKey(slotIndex))
                            {
                                // Return the index of the first available slot
                                return slotIndex;
                            }
                        }
                        else
                        {
                            // Return the first matching slot regardless of availability
                            return slotIndex;
                        }
                    }
                }

                // Update the cumulative index by adding the sizeLimit of the current slot
                cumulativeIndex += itemSlot.sizeLimit;
            }

            // Return -1 if no valid slot is found
            return -1;
        }

        #endregion

        private void CreateNewItem(InventoryItem item, int index)
        {
            var itemInfo = item.ItemInfo;
            itemInfo.ItemCollection = this;
            itemInfo.index = index;
            item.ItemInfo = itemInfo;

            _items.Add(index, item);
            Changed?.Invoke(item, true);
        }

        private void OverrideItem(InventoryItem item, int index)
        {
            var itemInfo = item.ItemInfo;
            itemInfo.ItemCollection = this;
            itemInfo.index = index;
            item.ItemInfo = itemInfo;

            _items[index] = item;
            Changed?.Invoke(_items[index], true);
        }

        private bool UpdateItem(InventoryItem item, int index)
        {
            if (_items[index].ItemTemplate.IsUnique)
                return false;

            if (_items[index].ItemInstance.ItemId != item.ItemInstance.ItemId)
                return false;

            _items[index].RemainingUses += item.ItemInstance.RemainingUses ?? 0;
            Changed?.Invoke(_items[index], true);
            return true;
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

        public void RemoveItem(InventoryItem item, int count)
        {
            foreach (var key in _items.Keys)
            {
                if (_items[key].ItemInstance.ItemInstanceId == item.ItemInstance.ItemInstanceId)
                {
                    if (_items[key].ItemInstance.RemainingUses <= count)
                    {
                        _items.Remove(key);
                        Changed?.Invoke(item, false);
                        return;
                    }

                    _items[key].ItemInstance.RemainingUses -= count;
                    Changed?.Invoke(item, true);
                    return;
                }
            }
        }

        private bool IsItemPermitted(InventoryItem item)
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

        private bool IsRestrictedByItemSlot(InventoryItem item, int slotIndex)
        {
            int cumulativeIndex = 0;

            if (itemSlots.Count == 0)
            {
                return true;
            }

            foreach (var itemSlot in itemSlots)
            {
                // Check if the slotIndex falls within the current itemSlot's range
                if (slotIndex >= cumulativeIndex && slotIndex < cumulativeIndex + itemSlot.sizeLimit)
                {
                    // Return true if the categories match, false otherwise
                    return itemSlot.itemCategory.Name == item.ItemTemplate.itemCategory.Name;
                }

                // Move the cumulative index forward by the size limit of the current itemSlot
                cumulativeIndex += itemSlot.sizeLimit;
            }

            return false; // Return false if no valid slot is found
        }

        public void EditorSetItemUser(ItemUser itemUser)
        {
            _itemUser = itemUser;
        }
    }
}