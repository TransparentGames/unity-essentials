
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransparentGames.Essentials.Data;
using TransparentGames.Essentials.Shop;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [Serializable]
    public class ItemCollection
    {
        public int SlotAmount => slotAmount;
        public string Category => category;
        public event Action<InventoryItem, bool> Changed;
        public IReadOnlyDictionary<int, InventoryItem> Items => _items;

        [SerializeField] private int slotAmount;
        [SerializeField] private string category;
        [SerializeField] private List<ItemRestriction> restrictions;

        private readonly Dictionary<int, InventoryItem> _items = new();

        public bool IsItemPermitted(InventoryItem item)
        {
            var isPermitted = restrictions.Count == 0;
            foreach (var restriction in restrictions)
            {
                if (restriction.IsSatisfiedBy(item))
                {
                    return true;
                }
            }

            return isPermitted;
        }

        public bool TryAddItem(InventoryItem item)
        {
            if (_items.Count >= slotAmount && item.ItemTemplate.IsUnique)
            {
                Debug.LogWarning("Inventory is full");
                return false;
            }

            if (IsItemPermitted(item) == false)
            {
                Debug.LogWarning("Item is not permitted");
                return false;
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

        public void AddItem(InventoryItem item, int index)
        {
            if (_items.TryGetValue(index, out var existingItem) == false)
            {
                CreateNewItem(item, index);
                return;
            }

            if (UpdateItem(item, index))
                return;

            Debug.LogWarning("ItemCollection: Overwriting item");
            OverrideItem(item, index);
        }

        private void CreateNewItem(InventoryItem item, int index)
        {
            item.ItemInfo = new ItemInfo
            {
                itemCollection = this,
            };
            _items.Add(index, item);
            Changed?.Invoke(item, true);
        }

        private void OverrideItem(InventoryItem item, int index)
        {
            item.ItemInfo = new ItemInfo
            {
                itemCollection = this,
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
    }
}