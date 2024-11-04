using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TransparentGames.Essentials.UI
{
    [RequireComponent(typeof(View))]
    public class CloseView : MonoBehaviour
    {
        [SerializeField] private List<InputActionReference> closeActions;
        [SerializeField] private InputActionReference cancelAction;

        private View _baseView;

        private void Awake()
        {
            _baseView = GetComponent<View>();
        }

        private void OnEnable()
        {
            if (cancelAction != null)
                cancelAction.action.performed += OnCancelActionPerformed;

            foreach (var closeAction in closeActions)
            {
                closeAction.action.Enable();
                closeAction.action.performed += OnCloseActionPerformed;
            }
        }

        private void OnDisable()
        {
            if (cancelAction != null)
                cancelAction.action.performed -= OnCancelActionPerformed;

            foreach (var closeAction in closeActions)
            {
                closeAction.action.performed -= OnCloseActionPerformed;
                closeAction.action.Disable();
            }
        }

        private void OnCancelActionPerformed(InputAction.CallbackContext context)
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.TryCancel(_baseView.State);
        }

        private void OnCloseActionPerformed(InputAction.CallbackContext context)
        {
            if (UIManager.InstanceExists)
                UIManager.Instance.TryClose(_baseView.State);
        }
    }
}