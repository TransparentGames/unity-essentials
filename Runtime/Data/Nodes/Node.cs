using System;
using UnityEngine;


namespace TransparentGames.Essentials.Data.Nodes
{
    /// <summary>
    /// Base class for getting the display value of a node
    /// </summary>
    public abstract class Node : ScriptableObject
    {
        public abstract string DisplayValue { get; }
        public abstract void AddListener(Action callback);
        public abstract void RemoveListener(Action callback);
    }
}