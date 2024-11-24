using System;
using System.Collections.Generic;
using TransparentGames.Essentials.Filters;
using TransparentGames.Essentials.UpdateManagement;
using UnityEngine;

namespace TransparentGames.Essentials.Detection
{
    public class FilterDetector : ComponentBase, IDetector
    {
        public Entity Owner => owner;
        public IReadOnlyList<IDetectable> AllDetected => _detected;

        public event Action Refreshed;
        public event Action<IDetectable> ObjectDetected;
        public event Action<IDetectable> ObjectLostDetection;

        [SerializeField] private int refreshFrameInterval = 10;
        [SerializeField] private List<AbstractScriptableObjectFilter> filters;
        public bool IsAnyDetected => AllDetected.Count > 0 && GetClosest() != null;

        private readonly List<IDetectable> _detected = new();
        private IUpdateEntity _updateEntity;
        private IDetectable _closest;


        public IDetectable GetClosest()
        {
            if ((_closest as UnityEngine.Object) == null)
                _closest = null;
            return _closest;
        }

        public void Clear()
        {
            for (int i = 0; i < _detected.Count; i++)
            {
                ObjectLostDetection?.Invoke(_detected[i]);
            }

            _detected.Clear();
        }

        private void OnEnable()
        {
            _updateEntity = UpdateManager.StartUpdate(Refresh, UpdateType.FixedUpdate, refreshFrameInterval);
        }

        private void OnDisable()
        {
            UpdateManager.Stop(_updateEntity);
        }

        protected virtual void Refresh()
        {
            UpdateDetected(filters);
            UpdateClosest();
            Refreshed?.Invoke();
        }

        protected void UpdateDetected(List<AbstractScriptableObjectFilter> filters)
        {
            var newDetected = DetectableRegistry.Instance.Detect(filters);

            for (int i = 0; i < _detected.Count; i++)
            {
                if (newDetected.Contains(_detected[i])) continue;
                ObjectLostDetection?.Invoke(_detected[i]);
                _detected.RemoveAt(i);
                i--;
            }

            for (int i = 0; i < newDetected.Count; i++)
            {
                if (_detected.Contains(newDetected[i])) continue;
                _detected.Add(newDetected[i]);
                ObjectDetected?.Invoke(newDetected[i]);
            }
        }

        private protected void UpdateClosest()
        {
            if (AllDetected.Count == 0)
            {
                _closest = null;
                return;
            }

            var closest = AllDetected[0];
            for (int i = 0; i < AllDetected.Count; i++)
            {
                if (Vector3.Distance(transform.position, AllDetected[i].Owner.transform.position) <
                    Vector3.Distance(transform.position, closest.Owner.transform.position))
                {
                    closest = AllDetected[i];
                }
            }

            _closest = closest;
        }
    }
}