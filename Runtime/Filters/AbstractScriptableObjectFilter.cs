using UnityEngine;

namespace TransparentGames.Essentials.Filters
{
    public abstract class AbstractScriptableObjectFilter : ScriptableObject
    {
        public abstract bool Check(Entity detectable);
    }
}