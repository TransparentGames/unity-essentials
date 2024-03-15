using UnityEngine;

namespace TransparentGames.Essentials
{
	/// <summary>
	/// Base class for ScriptableObjects that need a public description field.
	/// </summary>
	public class DescriptionBaseSO : ScriptableObjectWithId
	{
		[TextArea] public string description;
	}
}
