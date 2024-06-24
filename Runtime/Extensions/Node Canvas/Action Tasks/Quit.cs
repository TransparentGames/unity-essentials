using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Application")]
    public class Quit : ActionTask
    {
        protected override string info
        {
            get { return string.Format("Quit Application"); }
        }

        protected override void OnExecute()
        {
            Application.Quit();
            EndAction();
        }
    }
}