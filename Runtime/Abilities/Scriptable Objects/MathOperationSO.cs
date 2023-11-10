using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Essentials.Abilities.ScriptableObjects
{
    // We decided to further use abstraction here to enable the usage of diverse operation. 
    // Note that we can use graph like before, however by doing so we remove the ability to easily create a UI. 
    // You gotta try both way to see what you prefer.
    public abstract class MathOperationSO<T> : ScriptableObject
    {
        // The params keyword enable us to inject as much parameters as we want. 
        // A downside, is that it might be hard to use.
        // A graph would definitely make things easier here.
        public abstract T Execute(params object[] parameters);
    }
}
