
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    public class Inventory
    {
        public event Action<InventoryItem, bool> Changed;
        public IReadOnlyDictionary<int, InventoryItem> Items => _items;

        private readonly Dictionary<int, InventoryItem> _items;
        private int _slotAmount;
        public Inventory(int slotAmount)
        {
            _slotAmount = slotAmount;
            _items = new Dictionary<int, InventoryItem>(_slotAmount);
        }

        public bool AddItem(InventoryItem item)
        {
            if (_items.Count >= _slotAmount && item.ItemTemplate.IsUnique)
            {
                Debug.LogWarning("Inventory is full");
                return false;
            }

            for (int i = 0; i < _slotAmount; i++)
            {
                if (_items.ContainsKey(i))
                {
                    if (_items[i].ItemTemplate.IsUnique)
                        continue;

                    if (_items[i].ItemInstance.ItemId != item.ItemInstance.ItemId)
                        continue;

                    _items[i].ItemInstance.RemainingUses += item.ItemInstance.RemainingUses;
                    Changed?.Invoke(item, true);
                    return true;
                }

                _items.Add(i, item);
                Changed?.Invoke(item, true);
                return true;
            }

            return false;
        }

        public InventoryItem GetItem(int index)
        {
            if (!_items.ContainsKey(index))
            {
                return null;
            }

            return _items[index];
        }

        public List<InventoryItem> GetItems(string category)
        {
            var items = new List<InventoryItem>();
            foreach (var item in _items.Values)
            {
                if (item.ItemTemplate.itemClass == category)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public void RemoveItem(InventoryItem item)
        {
            foreach (var key in _items.Keys)
            {
                if (_items[key] == item)
                {
                    _items.Remove(key);
                    Changed?.Invoke(item, false);
                    return;
                }
            }
        }
    }
}