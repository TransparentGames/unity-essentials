using System;
using System.Collections.Generic;
using System.Reflection;
using TransparentGames.Essentials.Dummy;
using TransparentGames.Essentials.Items;
using TransparentGames.Essentials.Shop;
using UnityEngine;

public class InventorySystemManager : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemCollection;

    private static Dictionary<string, ItemTemplate> _itemTemplates = new();

    private void Awake()
    {
        if (itemCollection == null)
        {
            Debug.LogError("ItemCollection is not set in InventorySystemManager");
            return;
        }

        foreach (var itemTemplate in itemCollection.ItemTemplates)
        {
            _itemTemplates.Add(itemTemplate.itemId, itemTemplate);
        }
    }

    #region Items

    public static T CreateItem<T>(ItemInstance itemInstance) where T : InventoryItem
    {
        var itemTemplate = GetItemTemplate(itemInstance.ItemId);

        // Get the constructor that matches the parameters
        ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(ItemInstance), typeof(ItemTemplate) })
            ?? throw new InvalidOperationException($"Type {typeof(T)} does not have a constructor with parameters (ItemInstance, ItemTemplate).");

        // Create an instance of T using the constructor
        return (T)constructor.Invoke(new object[] { itemInstance, itemTemplate });
    }

    public static T RecreateItem<T>(ItemInstance itemInstance, ItemInfo itemInfo) where T : InventoryItem
    {
        var itemTemplate = GetItemTemplate(itemInstance.ItemId);

        // Get the constructor that matches the parameters
        ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(ItemInstance), typeof(ItemTemplate), typeof(ItemInfo) })
            ?? throw new InvalidOperationException($"Type {typeof(T)} does not have a constructor with parameters (ItemInstance, ItemTemplate, ItemInfo).");

        // Create an instance of T using the constructor
        return (T)constructor.Invoke(new object[] { itemInstance, itemTemplate, itemInfo });
    }

    public static bool MoveToCollectionItemAction(InventoryItem item, ItemCollection itemCollection)
    {
        if (item.ItemInfo.ItemCollection == itemCollection)
        {
            Debug.LogWarning("ISM.MoveToCollectionItemAction: Item is already in this collection");
            return false;
        }

        if (item.ItemInfo.ItemCollection.CanRemoveItem(item) == false)
            return false;

        if (itemCollection.CanAddItem(item) == false)
            return false;

        var itemCollectionCache = item.ItemInfo.ItemCollection;

        itemCollectionCache.RemoveItem(item);
        itemCollection.TryAddItem(item);

        return true;
    }

    public static bool MoveToSlotCollectionItemAction(InventoryItem item, ItemCollection itemCollection)
    {
        var potentialSlot = itemCollection.GetTargetSlotIndex(item, true);

        if (potentialSlot == -1)
        {
            potentialSlot = itemCollection.GetTargetSlotIndex(item, false);
            if (potentialSlot == -1)
            {
                Debug.LogWarning("ISM.MoveToSlotCollectionItemAction: No slot available");
                return false;
            }
        }

        var itemRemoved = item.ItemInfo.ItemCollection.RemoveItem(item.ItemInfo.index);

        var previousItem = itemCollection.GetItemAtSlot(potentialSlot);
        if (previousItem != null)
        {
            previousItem.ItemInfo.ItemCollection.RemoveItem(previousItem.ItemInfo.index);
            itemRemoved.ItemInfo.ItemCollection.AddItem(previousItem, itemRemoved.ItemInfo.index);
        }

        itemCollection.AddItem(itemRemoved, potentialSlot);
        return true;
    }

    public static ItemTemplate GetItemTemplate(string itemId)
    {
        if (_itemTemplates.ContainsKey(itemId))
        {
            return _itemTemplates[itemId];
        }

        Debug.LogError($"ItemTemplate with id [{itemId}] not found");
        return null;
    }

    #endregion

    public static bool CanAfford(Price price, InventoryItem currency)
    {
        return price.amount <= currency.RemainingUses;
    }

    public static void Pay(Price price, InventoryItem currency)
    {
        currency.RemainingUses -= price.amount;
    }
}