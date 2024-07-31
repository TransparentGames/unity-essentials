using System.Collections.Generic;
using System.Linq;
using TransparentGames.Essentials.Currency;
using TransparentGames.Essentials.Data.Nodes;
using TransparentGames.Essentials.Dummy;
using TransparentGames.Essentials.Items;
using UnityEngine;

public class InventorySystemManager : MonoBehaviour
{
    [SerializeField] private ItemCollection itemCollection;
    [SerializeField] private CurrencyCollection currencyCollection;

    private static Dictionary<string, ItemTemplate> _itemTemplates = new();
    private static Dictionary<string, CurrencyNode> _currenciesTemplates = new();

    private void Awake()
    {
        if (itemCollection == null || currencyCollection == null)
        {
            Debug.LogError("ItemCollection or CurrencyCollection is not set in InventorySystemManager");
            return;
        }

        foreach (var itemTemplate in itemCollection.ItemTemplates)
        {
            _itemTemplates.Add(itemTemplate.itemId, itemTemplate);
        }

        foreach (var currencyNode in currencyCollection.CurrencyNodes)
        {
            _currenciesTemplates.Add(currencyNode.itemId, currencyNode);
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

    public static ItemTemplate GetItemTemplate(string itemId)
    {
        if (_itemTemplates.ContainsKey(itemId))
        {
            return _itemTemplates[itemId];
        }

        Debug.LogError($"ItemTemplate with id [{itemId}] not found");
        return null;
    }

    public static List<InventoryItem> GetItemCategory(string category)
    {
        return InventoryManager.Instance.InventoryItems.Values.ToList().FindAll(item => item.ItemInstance.ItemClass == category);
    }
    #endregion

    #region Currency

    public static CurrencyNode GetCurrency(string currencyId)
    {
        if (_currenciesTemplates.ContainsKey(currencyId))
        {
            return _currenciesTemplates[currencyId];
        }

        Debug.LogError($"CurrencyNode with id [{currencyId}] not found");
        return null;
    }

    #endregion
}