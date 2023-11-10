using UnityEngine;

namespace Essentials.Abilities.ScriptableObjects
{
	/// <summary>
	/// Base class for Abilities ScriptableObjects that need a public description field.
	/// </summary>
	public class DescriptionAbilityBaseSO : ScriptableObject
	{
		[TextArea] public string description;
	}

}