
using System;
using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.UI
{
    [RequireComponent(typeof(Canvas))]
    public class DynamicElementsCanvas : MonoSingleton<DynamicElementsCanvas>
    {
        public Canvas Canvas { get; private set; }

        public enum Layer
        {
            Background,
            Default,
            Foreground
        }

        private readonly List<RectTransform> _layers = new();

        public RectTransform GetTransform(Layer layer = Layer.Default)
        {
            return _layers[(int)layer];
        }

        public new Transform transform => GetTransform(Layer.Default);

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

            Canvas = GetComponent<Canvas>();

            AssignCamera();
        }

        private void AssignCamera()
        {
            UICamera.Initialized(() => Canvas.worldCamera = UICamera.Instance.Camera);
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