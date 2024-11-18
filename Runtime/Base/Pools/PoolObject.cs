using System;
using TransparentGames.Essentials;
using UnityEngine;
using UnityEngine.Pool;

namespace TransparentGames.Essentials.Pools
{
    /// <summary>
    ///     Create your Pooled Object MonoBehaviour classes from this.
    /// </summary>
    public abstract class PoolObject : MonoBehaviour
    {
        public IPoolData PoolData { get; set; }
        public abstract void Release();
    }
}