using System;
using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.UI.ScreenSpace
{
    [RequireComponent(typeof(Canvas))]
    public class ScreenSpaceCanvas : MonoSingleton<ScreenSpaceCanvas>
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
            foreach (var order in Enum.GetNames(typeof(Layer)))
            {
                Transform trans = new GameObject(order).transform;
                trans.SetParent(transform);
                trans.localScale = Vector3.one;
                _layers.Add(trans);
            }

            base.Awake();

            _canvas = GetComponent<Canvas>();
        }

        public Transform Transform => GetTransform(Layer.Default);

        public Transform GetTransform(Layer layer = Layer.Default)
        {
            return _layers[(int)layer];
        }
    }
}