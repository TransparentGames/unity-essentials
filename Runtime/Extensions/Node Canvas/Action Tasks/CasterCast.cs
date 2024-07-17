using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using TransparentGames.Essentials.Abilities;

namespace TransparentGames.Essentials.Extensions
{
    [Category("Abilities")]
    public class CasterCast : ActionTask
    {
        public BBParameter<Caster> Caster;
        public BBParameter<GameObject> Target;
        public bool waitUntilFinish = false;

        protected override string OnInit()
        {
            if (Caster.value == null)
                return "Caster is null";

            if (Target.value == null)
                return "Target is null";

            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            Caster.value.Cast(Target.value);
            if (!waitUntilFinish)
                EndAction(true);
            else
            {
                Caster.value.Ready += OnCasterReady;
            }
        }

        protected void OnCasterReady()
        {
            Caster.value.Ready -= OnCasterReady;
            EndAction(true);
        }
    }
}