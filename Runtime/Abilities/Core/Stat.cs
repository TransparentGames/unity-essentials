using System.Collections;
using System.Collections.Generic;
using Essentials.Abilities.ScriptableObjects;
using UnityEngine;

namespace Essentials.Abilities
{
    public abstract class Stat<T> : IStat
    {
        // This type we do not instantiate the stats from the definition as we want 
        // to serialize the data from the character/potential other object. Otherwise, we would need to create a definition per stats which would result in a lot of definition. 
        [SerializeField] private StatDefinitionSO definition;

        public abstract T GetValue();
        public abstract void SetValue(T value);

        //This is called whenever the holder has been instantiated. If we were using Component instead of POCO, we would have the possibility to use Awake/Start etc. In a game where there is little to no ennemies or the performance are not important (Turn Base), using component could actually be better in term of maintanability. 
        public abstract void Initialize();
    }
}
