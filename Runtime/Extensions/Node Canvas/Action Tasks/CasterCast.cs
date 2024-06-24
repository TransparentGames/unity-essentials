using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using TransparentGames.Abilities;

namespace TransparentGames.Extensions
{
    [Category("Abilities")]
    public class CasterCast : ActionTask
    {
        public BBParameter<Caster> Caster;
        public BBParameter<Vector3> Target;

        protected override string OnInit()
        {
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            Caster.value.Cast(Target.value);
            EndAction(true);
        }

        protected override void OnUpdate()
        {

        }
    }
}