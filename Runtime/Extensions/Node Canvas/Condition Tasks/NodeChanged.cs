using ParadoxNotion.Design;
using UnityEngine.UI;
using UnityEngine;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{

    [Category("Node")]
    public class NodeChanged : ConditionTask
    {
        [RequiredField]
        public BBParameter<TransparentGames.Essentials.Data.Nodes.Node> node;

        protected override string info
        {
            get { return string.Format("Node {0} Changed", node.ToString()); }
        }

        protected override string OnInit()
        {
            return null;
        }

        protected override bool OnCheck() { return false; }
        void OnChanged()
        {
            YieldReturn(true);
        }

        protected override void OnEnable()
        {
            node.value.AddListener(OnChanged);
        }

        protected override void OnDisable()
        {
            node.value.RemoveListener(OnChanged);
        }
    }
}