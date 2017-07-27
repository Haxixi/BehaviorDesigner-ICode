using UnityEngine;
using BehaviorDesigner.Runtime;
using ICode;

namespace BehaviorDesigner.Runtime.Tasks.ICode
{
    [TaskDescription("Start executing a ICode State Machine. The task will stay in a running state until the State Machine has returned success or failure. " +
                     "The State Machine must finish with a Resume From ICode action.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=56")]
    [TaskCategory("ICode")]
    [TaskIcon("Assets/Behavior Designer/Third Party/ICode/Editor/ICodeIcon.png")]
    public class StartStateMachine : Action
    {
        [Tooltip("The GameObject that the ICode FSM component is attached to")]
        public SharedGameObject targetGameObject;
        [Tooltip("The group of the state machine behaviour component. This allows you to have multiple state machines on a single GameObject")]
        public SharedInt group = 0;
        [Tooltip("Should the task wait for the state machine to complete its execution?")]
        public SharedBool waitForStateMachineCompletion = true;
        [Tooltip("Should the local variables be synchronized between Behavior Designer and ICode?")]
        public SharedBool synchronizeVariables = false;
        [Tooltip("Should the global variables be synchronized between Behavior Designer and ICode?")]
        public SharedBool synchronizeGlobalVariables = false;

        // A cache of the iCodeBehavior
        private ICodeBehaviour iCodeBehavior;
        public ICodeBehaviour ICodeBehavior { get { return iCodeBehavior; } }
        // The return status of the state machine after it has finished executing
        private TaskStatus status;

        public override void OnAwake()
        {
            // Find the correct iCodeBehavior based on the name.
            var iCodeBehaviorComponents = targetGameObject.Value != null ? targetGameObject.Value.GetComponents<ICodeBehaviour>() : gameObject.GetComponents<ICodeBehaviour>();
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

        public override void OnStart()
        {
            // Tell the Behavior Manager that we are running a ICodeBehaviour instance.
            if (iCodeBehavior != null && (!waitForStateMachineCompletion.Value || BehaviorManager.instance.MapObjectToTask(iCodeBehavior, this, BehaviorManager.ThirdPartyObjectType.ICode))) {
                if (waitForStateMachineCompletion.Value) {
                    status = TaskStatus.Running;
                } else {
                    status = TaskStatus.Success;
                }

                // Synchronize variables
                if (synchronizeVariables.Value) {
                    BehaviorManager.instance.SyncVariablesToICode(Owner.GetBehaviorSource(), iCodeBehavior.stateMachine.Variables);
                }
                if (synchronizeGlobalVariables.Value) {
                    BehaviorManager.instance.SyncGlobalVariablesToICode();
                }

                iCodeBehavior.EnableStateMachine();
                iCodeBehavior.enabled = true;
            } else {
                // If something went wrong then return failure.
                status = TaskStatus.Failure;
            }
        }

        public override TaskStatus OnUpdate()
        {
            // We are returning the same status until we hear otherwise.
            return status;
        }

        public override void OnPause(bool paused)
        {
            if (iCodeBehavior != null) {
                iCodeBehavior.DisableStateMachine(paused);
            }
        }

        // The ICode action ResumeFromICode will call this function when it has completed. 
        public void ICodeFinished(TaskStatus stateMachineStatus)
        {
            // Update the status with what ICode returned.
            status = stateMachineStatus;
        }

        public override void OnEnd()
        {
            if (iCodeBehavior == null)
                return;

            // Synchronize variables
            if (synchronizeVariables.Value) {
                BehaviorManager.instance.SyncVariablesFromICode(Owner.GetBehaviorSource(), iCodeBehavior.stateMachine.Variables);
            }
            if (synchronizeGlobalVariables.Value) {
                BehaviorManager.instance.SyncGlobalVariablesFromICode();
            }
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            targetGameObject = null;
            group = 0;
            waitForStateMachineCompletion = true;
            synchronizeVariables = synchronizeGlobalVariables = false;
        }
    }
}
