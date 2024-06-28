using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.UI.ScreenSpace
{
    public class UICamera : MonoSingleton<UICamera>
    {
        private Camera _camera;

        public Camera Camera
        {
            get
            {
                if (_camera == null) _camera = GetComponent<Camera>();
                return _camera;
            }
        }
    }
}