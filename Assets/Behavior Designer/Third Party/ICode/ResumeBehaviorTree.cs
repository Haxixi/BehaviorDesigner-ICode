using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace ICode.Actions
{
    [Tooltip("Resumes an already executing behavior tree.")]
    [System.Serializable]
    public class ResumeBehaviorTree : StateAction
    {
        [Tooltip("Was the State Machine a success?")]
        public FsmBool success;

        public override void OnEnter()
        {
            // Let the Behavior Manager know that the State Machine is done
            BehaviorManager.instance.ICodeFinished(Root.Owner, (success.Value ? TaskStatus.Success : TaskStatus.Failure));

            Finish();
        }
    }
}