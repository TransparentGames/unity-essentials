using System;
using TransparentGames.Essentials;
using UnityEngine;


namespace TransparentGames.Data
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