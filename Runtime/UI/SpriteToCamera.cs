
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class SpriteToCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            AlignCamera();
        }

        private void AlignCamera()
        {
            if (Camera.main != null)
            {
                var camXForm = Camera.main.transform;
                var forward = transform.position - camXForm.position;
                forward.Normalize();
                var up = Vector3.Cross(forward, camXForm.right);
                transform.rotation = Quaternion.LookRotation(forward, up);
            }
        }
    }
}
