#if DISABLE_PLAYFABCLIENT_API || !ENABLE_PLAYFABCLIENT_API

using System;
using System.Collections.Generic;

namespace TransparentGames.Essentials.Dummy
{
    public class GrantedItemInstance
    {
        public string ItemInstanceId;
        public string ItemId;
        public string DisplayName;
        public string ItemClass;
        public DateTime? PurchaseDate;
        public DateTime? Expiration;
        public int? RemainingUses;
        public int? UsesIncrementedBy;
        public string Annotation;
        public string CatalogVersion;
        public string BundleParent;
        public string UnitCurrency;
        public uint UnitPrice;
        public List<string> BundleContents;
        public Dictionary<string, string> CustomData;
    }

    public class ItemInstance
    {
        public string ItemInstanceId;
        public string ItemId;
        public string DisplayName;
        public string ItemClass;
        public DateTime? PurchaseDate;
        public DateTime? Expiration;
        public int? RemainingUses;
        public int? UsesIncrementedBy;
        public string Annotation;
        public string CatalogVersion;
        public string BundleParent;
        public string UnitCurrency;
        public uint UnitPrice;
        public List<string> BundleContents;
        public Dictionary<string, string> CustomData;
    }
}

#endif