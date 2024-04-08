using System.Collections.Generic;
using TransparentGames.Essentials.Filters;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.Detection
{
    public class DetectableRegistry : PersistentMonoSingleton<DetectableRegistry>
    {
        public List<IDetectable> AllDetected => _detected;

        private readonly List<IDetectable> _detected = new();

        public void Register(IDetectable detectable)
        {
            if (_detected.Contains(detectable)) return;

            _detected.Add(detectable);
        }

        public static void UnRegister(IDetectable detectable)
        {
            if (InstanceExists)
                Instance._detected.Remove(detectable);
        }

        public List<IDetectable> Detect(List<AbstractScriptableObjectFilter> filters)
        {
            var list = new List<IDetectable>();
            for (int i = 0; i < _detected.Count; i++)
            {
                if (IsPassingFilters(_detected[i], filters))
                    list.Add(_detected[i]);
            }

            return list;
        }

        private bool IsPassingFilters(IDetectable detectable, List<AbstractScriptableObjectFilter> filters)
        {
            for (int i = 0; i < filters.Count; i++)
            {
                if (filters[i].Check(detectable.Owner) == false) return false;
            }

            return true;
        }

        public List<IDetectable> GetInRange(Vector3 position, float range)
        {
            List<IDetectable> list = new();
            for (int i = 0; i < _detected.Count; i++)
            {
                if (!IsInRange(position, range, _detected[i]))
                    continue;

                list.Add(_detected[i]);
            }

            return list;
        }

        public List<IDetectable> OverlapBox(Vector3 position, Vector3 size, Quaternion rotation)
        {
            List<IDetectable> list = new();

            foreach (IDetectable detectable in _detected)
            {
                Vector3 objectPosition = detectable.Owner.transform.position;

                Vector3 halfSize = size / 2;

                Vector3 localPosition = Quaternion.Inverse(rotation) * (objectPosition - position);

                if (Mathf.Abs(localPosition.x) < halfSize.x && Mathf.Abs(localPosition.y) < halfSize.y)
                {
                    list.Add(detectable);
                }
            }

            return list;
        }

        private bool IsInRange(Vector3 position, float range, IDetectable detectable)
        {
            return Vector3.Distance(position, detectable.Owner.transform.position) <= range;
        }
    }
}