using TransparentGames.Core;
using UnityEngine;

/// <summary>
/// Base class for ScriptableObjects that need a public description field.
/// </summary>
public class DescriptionBaseSO : ScriptableObjectWithId
{
	[TextArea] public string description;
}
