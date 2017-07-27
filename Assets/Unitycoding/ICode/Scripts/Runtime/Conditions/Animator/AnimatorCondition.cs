using UnityEngine;
using System.Collections;

namespace ICode.Conditions.UnityAnimator{
	[System.Serializable]
	public abstract class AnimatorCondition : Condition {
		[SharedPersistent]
		[Tooltip("GameObject to test.")]
		public FsmGameObject gameObject;

		protected Animator animator;
		
		public override void OnEnter ()
		{
			animator = gameObject.Value.GetComponent<Animator> ();
		}
	}
}