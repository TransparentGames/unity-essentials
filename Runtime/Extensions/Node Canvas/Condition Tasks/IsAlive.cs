using NodeCanvas.Framework;
using ParadoxNotion.Design;
using TransparentGames.Essentials.Combat;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions
{
    [Category("Health")]
    public class IsAlive : ConditionTask
    {
        public BBParameter<IHealth> health;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            if (health.value == null)
            {
                return "IHealth is null";
            }

            return null;
        }

        //Called whenever the condition gets enabled.
        protected override void OnEnable()
        {

        }

        //Called whenever the condition gets disabled.
        protected override void OnDisable()
        {

        }

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck()
        {
            return health.value.CurrentHealth > 0;
        }
    }
}