
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

        public void AddItem(InventoryItem item)
        {
            if (_items.Count >= _slotAmount)
            {
                Debug.LogWarning("Inventory is full");
                return;
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
                    return;
                }

                _items.Add(i, item);
                Changed?.Invoke(item, true);
                return;
            }
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
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] == item)
                {
                    _items[i] = null;
                    Changed?.Invoke(item, false);
                    return;
                }
            }
        }
    }
}