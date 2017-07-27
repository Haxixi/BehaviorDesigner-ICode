using UnityEngine;
using BehaviorDesigner.Runtime;
using ICode;

namespace BehaviorDesigner.Runtime.Tasks.ICode
{
    [TaskDescription("Run a ICode State machine that completes within the same frame. If the State Machine does not complete in time then the task will return failure. " +
                     "The State Machine must finish with a Resume From ICode action.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=56")]
    [TaskCategory("ICode")]
    [TaskIcon("Assets/Behavior Designer/Third Party/ICode/Editor/ICodeIcon.png")]
    public class RunConditionalStateMachine : Action
    {
        [Tooltip("The GameObject that the ICode component is attached to")]
        public SharedGameObject targetGameObject;
        [Tooltip("The group of the state machine behaviour component. This allows you to have multiple state machines on a single GameObject")]
        public SharedInt group = 0;

        // A cache of the iCodeBehavior
        private ICodeBehaviour iCodeBehavior;
        public ICodeBehaviour ICodeBehavior { get { return iCodeBehavior; } }
        // The return status of the state machine after it has finished executing
        private TaskStatus status;

        public override void OnAwake()
        {
            // Find the correct iCodeBehavior based on the name.
            var iCodeBehaviorComponents = targetGameObject != null ? targetGameObject.Value.GetComponents<ICodeBehaviour>() : gameObject.GetComponents<ICodeBehaviour>();
            if (iCodeBehaviorComponents != null && iCodeBehaviorComponents.Length > 0) {
                iCodeBehavior = iCodeBehaviorComponents[0];
                //  We don't need the group if there is only one State Machine Behaviour component
                if (iCodeBehaviorComponents.Length > 1) {
                    for (int i = 0; i < iCodeBehaviorComponents.Length; ++i) {
                        if (iCodeBehaviorComponents[i].group == group.Value) {
                            // Cache the result when we have a match and stop looping.
                            iCodeBehavior = iCodeBehaviorComponents[i];
                            break;
                        }
                    }
                }
            }

            // We can't do much if there isn't a ICodeBehaviour.
            if (iCodeBehavior == null) {
                Debug.LogError(string.Format("Unable to find State Machine Behaviour with group {0}{1}", group, (targetGameObject.Value != null ? string.Format(" attached to {0}", targetGameObject.Value.name) : "")));
            }
        }

        public override TaskStatus OnUpdate()
        {
            // Tell the Behavior Manager that we are running a State Machine instance.
            if (iCodeBehavior != null && BehaviorManager.instance.MapObjectToTask(iCodeBehavior, this, BehaviorManager.ThirdPartyObjectType.ICode)) {
                status = TaskStatus.Failure;

                iCodeBehavior.EnableStateMachine();
                iCodeBehavior.enabled = true;

                // Remove the task from the mapping immediately after the event is called. If the state machine hasn't returned by now it is running longer than one frame and has lost its chance to return
                BehaviorManager.instance.RemoveActiveThirdPartyTask(this);
            } else {
                // If something went wrong then return failure.
                status = TaskStatus.Failure;
            }
            return status;
        }

        // The ICode action ResumeFromICode will call this function when it has completed. 
        public void ICodeFinished(TaskStatus stateMachineStatus)
        {
            // Update the status with what ICode returned.
            status = stateMachineStatus;
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            targetGameObject = null;
            group = 0;
        }
    }
}
