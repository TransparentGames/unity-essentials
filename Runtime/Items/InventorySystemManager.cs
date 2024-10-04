using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TransparentGames.Essentials.Currency;
using TransparentGames.Essentials.Data;
using TransparentGames.Essentials.Data.Nodes;
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

    public static InventoryItem CreateItem(string itemId, int count = 1)
    {
        var itemTemplate = GetItemTemplate(itemId);
        var itemInstance = new ItemInstance
        {
            ItemId = itemId,
            RemainingUses = count,
            ItemInstanceId = System.Guid.NewGuid().ToString(),
            ItemClass = itemTemplate.itemClass
        };

        return new InventoryItem(itemInstance, itemTemplate);
    }

    public static bool MoveToCollectionItemAction(InventoryItem item, ItemCollection itemCollection)
    {
        if (item.ItemInfo.itemCollection == itemCollection)
        {
            Debug.LogWarning("ISM.MoveToCollectionItemAction: Item is already in this collection");
            return false;
        }

        if (item.ItemInfo.itemCollection.CanRemoveItem(item) == false)
            return false;

        if (itemCollection.TryAddItem(item) == false)
            return false;

        item.ItemInfo.itemCollection.RemoveItem(item);
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