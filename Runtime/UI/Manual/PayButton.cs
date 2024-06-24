using UnityEngine;

public abstract class PayButton : ManualButton
{
    public abstract bool CanAfford { get; }
    public abstract int Price { get; }

    protected override void OnButtonClicked()
    {
        if (CanAfford == false)
            return;

        base.OnButtonClicked();
    }
}