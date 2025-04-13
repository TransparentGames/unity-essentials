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
                var trans = new GameObject(order).transform;
                trans.SetParent(base.transform);
                trans.localScale = Vector3.one;
                trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, 0);
                var rectTransform = trans.gameObject.AddComponent<RectTransform>();
                StretchToEdges(ref rectTransform);
                _layers.Add(rectTransform);
            }

            base.Awake();

            _canvas = GetComponent<Canvas>();
        }

        public Transform Transform => GetTransform(Layer.Default);

        public Transform GetTransform(Layer layer = Layer.Default)
        {
            return _layers[(int)layer];
        }

        private void StretchToEdges(ref RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }
}