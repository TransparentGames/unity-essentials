using System;
using UnityEngine;

public class GameObjectPoolItem : MonoBehaviour
{
    public event Action<GameObjectPoolItem> ReturnToPool;

    public void OnReturnToPool()
    {
        ReturnToPool?.Invoke(this);
    }
}