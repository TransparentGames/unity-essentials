using System;
using TransparentGames.Essentials;
using UnityEngine;

namespace TransparentGames.Data
{
    /// <summary>
    /// Base class for modifying the value of a node, if you want to just get display value (string), use <see cref="Node"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseNode<T> : Node
    {
        public abstract T Value { get; set; }
    }
}