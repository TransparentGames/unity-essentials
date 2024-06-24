using System;
using UnityEngine;
using UnityEngine.UI;

public class ManualButton : MonoBehaviour
{
    public event Action Success;
    public event Action Clicked;

    [SerializeField] private Button button;
    [SerializeField] private bool isAutoProcess = true;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnEnable()
    {
        Unlock();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    protected virtual void OnButtonClicked()
    {
        Clicked?.Invoke();

        if (isAutoProcess)
            Process();
    }

    public void Process(Action processAction = null)
    {
        button.interactable = false;
        processAction?.Invoke();
        Success?.Invoke();

        if (isAutoProcess)
            Unlock();
    }

    public void Unlock()
    {
        button.interactable = true;
    }
}