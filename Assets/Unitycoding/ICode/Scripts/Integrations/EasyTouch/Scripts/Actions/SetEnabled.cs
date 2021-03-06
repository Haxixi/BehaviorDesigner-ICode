﻿#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class SetEnabled : StateAction {
		[InspectorLabel("Enable")]
		public FsmBool mEnable;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter()
		{
			EasyTouch.SetEnabled (mEnable.Value);
			if (!everyFrame) {
				Finish();			
			}
		}

		public override void OnUpdate ()
		{
			EasyTouch.SetEnabled (mEnable.Value);
		}
	}
}
#endif