using System;
using UnityEngine;
using UnityEngine.UI;

public class ManualToggle : MonoBehaviour
{
    public event Action<bool> Success;
    public event Action<bool> Toggled;

    [SerializeField] private Button button;
    [SerializeField] private Image onImage;
    [SerializeField] private Image offImage;

    private bool _isOn = false;

    private void Awake()
    {
        button.onClick.AddListener(OnToggleValueChanged);
    }

    private void OnEnable()
    {
        Redraw();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged()
    {
        Toggled?.Invoke(!_isOn);
    }

    private void Redraw()
    {
        onImage.gameObject.SetActive(_isOn);
        offImage.gameObject.SetActive(!_isOn);
    }

    public void Override(bool value)
    {
        _isOn = value;
        Redraw();
    }

    public void Process(Action processAction = null)
    {
        button.interactable = false;
        processAction?.Invoke();
        Override(!_isOn);
        Success?.Invoke(_isOn);
        button.interactable = true;
    }
}