using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TransparentGames.Essentials.UI
{
    [RequireComponent(typeof(View))]
    public class CloseView : MonoBehaviour
    {
        [SerializeField] private List<InputActionReference> closeActions;

        private View _baseView;

        private void Awake()
        {
            _baseView = GetComponent<View>();
        }

        private void OnEnable()
        {
            foreach (var closeAction in closeActions)
            {
                closeAction.action.Enable();
                closeAction.action.performed += OnCloseActionPerformed;
            }
        }

        private void OnDisable()
        {
            foreach (var closeAction in closeActions)
            {
                closeAction.action.performed -= OnCloseActionPerformed;
                closeAction.action.Disable();
            }
        }

        private void OnCloseActionPerformed(InputAction.CallbackContext context)
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.TryClose(_baseView.State);
        }
    }
}