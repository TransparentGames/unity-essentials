using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    /// <summary>
    /// Base class for all storage based items.
    /// </summary>
    public abstract class PlayfabItemTemplate : ScriptableObject
    {
        /// <summary>
        /// The unique identifier of the item.
        /// </summary>
        public string itemId;
    }
}