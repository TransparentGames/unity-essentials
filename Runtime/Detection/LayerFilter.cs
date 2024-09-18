using TransparentGames.Essentials;
using TransparentGames.Essentials.Filters;
using UnityEngine;

[CreateAssetMenu(fileName = "Layer Filter", menuName = "Transparent Games/Detection/Layer Filter", order = 0)]
public class LayerFilter : AbstractScriptableObjectFilter
{
    [SerializeField] private LayerMask layerMask;

    public override bool Check(Entity detectable)
    {
        return (layerMask.value & (1 << detectable.gameObject.layer)) != 0;
    }
}