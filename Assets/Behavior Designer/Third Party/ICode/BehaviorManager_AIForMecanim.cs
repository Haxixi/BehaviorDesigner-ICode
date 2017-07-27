using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.ICode;
using ICode;

namespace BehaviorDesigner.Runtime
{
    public static class BehaviorManager_ICode
    {
        public static void ICodeFinished(this BehaviorManager behaviorManager, ICodeBehaviour iCodeBehavior, TaskStatus status)
        {
            if (behaviorManager == null) {
                return;
            }

            var task = behaviorManager.TaskForObject(iCodeBehavior);
            if (task is Tasks.ICode.StartStateMachine) {
                var iCodeTask = task as Tasks.ICode.StartStateMachine;
                iCodeTask.ICodeFinished(status);
            } else if (task is RunConditionalStateMachine) {
                var iCodeTask = task as RunConditionalStateMachine;
                iCodeTask.ICodeFinished(status);
            }
        }

        public static bool StopICode(Tasks.ICode.StartStateMachine stateMachineTask)
        {
            var iCodeBehavior = stateMachineTask.ICodeBehavior;
            if (iCodeBehavior != null) {
                iCodeBehavior.DisableStateMachine(false);
            }
            return true;
        }

        public static void SyncVariablesToICode(this BehaviorManager behaviorManager, IVariableSource variableSource, FsmVariable[] iCodeVariables)
        {
            DoVariablesToICodeSync(variableSource, iCodeVariables);
        }

        public static void SyncGlobalVariablesToICode(this BehaviorManager behaviorManager)
        {
            DoVariablesToICodeSync(GlobalVariables.Instance, ICode.GlobalVariables.GetVariables());
        }

        private static void DoVariablesToICodeSync(IVariableSource variableSource, FsmVariable[] iCodeVariables)
        {
            if (variableSource == null)
                return;

            SharedVariable behaviorDesignerVariable = null;
            FsmVariable iCodeVariable = null;

            for (int i = 0; i < iCodeVariables.Length; ++i) {
                iCodeVariable = iCodeVariables[i];
                if ((behaviorDesignerVariable = variableSource.GetVariable(iCodeVariable.Name)) != null) {

                    // FsmInt
                    if (iCodeVariable is FsmInt) {
                        if (behaviorDesignerVariable is SharedInt) {
                            (iCodeVariable as FsmInt).Value = (int)behaviorDesignerVariable.GetValue();
                        }
                        continue;
                    }

                    // FsmFloat
                    if (iCodeVariable is FsmFloat) {
                        if (behaviorDesignerVariable is SharedFloat) {
                            (iCodeVariable as FsmFloat).Value = (float)behaviorDesignerVariable.GetValue();
                        }
                        continue;
                    }

                    // FsmBool
                    if (iCodeVariable is FsmBool) {
                        if (behaviorDesignerVariable is SharedBool) {
                            (iCodeVariable as FsmBool).Value = (bool)behaviorDesignerVariable.GetValue();
                        }
                        continue;
                    }

                    // FsmString
                    if (iCodeVariable is FsmString) {
                        if (behaviorDesignerVariable is SharedString) {
                            (iCodeVariable as FsmString).Value = (string)behaviorDesignerVariable.GetValue();
                        }
                        continue;
                    }

                    // FsmColor
                    if (iCodeVariable is FsmColor) {
                        if (behaviorDesignerVariable is SharedColor) {
                            (iCodeVariable as FsmColor).Value = (Color)behaviorDesignerVariable.GetValue();
                        }
                        continue;
                    }

                    // FsmVector2
                    if (iCodeVariable is FsmVector2) {
                        if (behaviorDesignerVariable is SharedVector2) {
                            (iCodeVariable as FsmVector2).Value = (Vector2)behaviorDesignerVariable.GetValue();
                        }
                        continue;
                    }

                    // FsmVector3
                    if (iCodeVariable is FsmVector3) {
                        if (behaviorDesignerVariable is SharedVector3) {
                            (iCodeVariable as FsmVector3).Value = (Vector3)behaviorDesignerVariable.GetValue();
                        }
                        continue;
                    }

                    // FsmObject
                    if (iCodeVariable is FsmObject) {
                        if (behaviorDesignerVariable is SharedGameObject) {
                            (iCodeVariable as FsmObject).Value = (GameObject)behaviorDesignerVariable.GetValue();
                        } else if (behaviorDesignerVariable is SharedObject) {
                            (iCodeVariable as FsmObject).Value = (Object)behaviorDesignerVariable.GetValue();
                        }
                        continue;
                    }
                }
            }
        }

        public static void SyncVariablesFromICode(this BehaviorManager behaviorManager, IVariableSource variableSource, FsmVariable[] iCodeVariables)
        {
            DoVariablesFromICodeSync(variableSource, iCodeVariables);
        }

        public static void SyncGlobalVariablesFromICode(this BehaviorManager behaviorManager)
        {
            DoVariablesFromICodeSync(GlobalVariables.Instance, ICode.GlobalVariables.GetVariables());
        }

        private static void DoVariablesFromICodeSync(IVariableSource variableSource, FsmVariable[] iCodeVariables)
        {
            if (variableSource == null)
                return;

            SharedVariable behaviorDesignerVariable = null;
            FsmVariable iCodeVariable = null;

            for (int i = 0; i < iCodeVariables.Length; ++i) {
                iCodeVariable = iCodeVariables[i];
                if ((behaviorDesignerVariable = variableSource.GetVariable(iCodeVariable.Name)) != null) {

                    // FsmInt
                    if (iCodeVariable is FsmInt) {
                        if (behaviorDesignerVariable is SharedInt) {
                            behaviorDesignerVariable.SetValue((iCodeVariable as FsmInt).Value);
                        }
                        continue;
                    }

                    // FsmFloat
                    if (iCodeVariable is FsmFloat) {
                        if (behaviorDesignerVariable is SharedFloat) {
                            behaviorDesignerVariable.SetValue((iCodeVariable as FsmFloat).Value);
                        }
                        continue;
                    }

                    // FsmBool
                    if (iCodeVariable is FsmBool) {
                        if (behaviorDesignerVariable is SharedBool) {
                            behaviorDesignerVariable.SetValue((iCodeVariable as FsmBool).Value);
                        }
                        continue;
                    }

                    // FsmString
                    if (iCodeVariable is FsmString) {
                        if (behaviorDesignerVariable is SharedString) {
                            behaviorDesignerVariable.SetValue((iCodeVariable as FsmString).Value);
                        }
                        continue;
                    }

                    // FsmColor
                    if (iCodeVariable is FsmColor) {
                        if (behaviorDesignerVariable is SharedColor) {
                            behaviorDesignerVariable.SetValue((iCodeVariable as FsmColor).Value);
                        }
                        continue;
                    }

                    // FsmVector2
                    if (iCodeVariable is FsmVector2) {
                        if (behaviorDesignerVariable is SharedVector2) {
                            behaviorDesignerVariable.SetValue((iCodeVariable as FsmVector2).Value);
                        }
                        continue;
                    }

                    // FsmVector3
                    if (iCodeVariable is FsmVector3) {
                        if (behaviorDesignerVariable is SharedVector3) {
                            behaviorDesignerVariable.SetValue((iCodeVariable as FsmVector3).Value);
                        }
                        continue;
                    }

                    // FsmObject
                    if (iCodeVariable is FsmObject) {
                        if (behaviorDesignerVariable is SharedGameObject || behaviorDesignerVariable is SharedObject) {
                            behaviorDesignerVariable.SetValue((iCodeVariable as FsmObject).Value);
                        }
                        continue;
                    }
                }
            }
        }
    }
}