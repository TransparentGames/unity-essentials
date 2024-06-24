using System;
using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class WorldSpaceCanvas : MonoSingleton<WorldSpaceCanvas>
{
    // TODO: Auto setup camera here
    public enum Layer
    {
        Background,
        Default,
        Foreground
    }

    private readonly List<Transform> _layers = new();
    private Canvas _canvas;

    protected override void Awake()
    {
        base.Awake();
        foreach (var order in Enum.GetNames(typeof(Layer)))
        {
            Transform trans = new GameObject(order).transform;
            trans.SetParent(transform);
            trans.localScale = Vector3.one;
            _layers.Add(trans);
        }

        _canvas = GetComponent<Canvas>();
    }

    public Transform Transform => GetTransform(Layer.Default);

    public Transform GetTransform(Layer layer)
    {
        return _layers[(int)layer];
    }
}