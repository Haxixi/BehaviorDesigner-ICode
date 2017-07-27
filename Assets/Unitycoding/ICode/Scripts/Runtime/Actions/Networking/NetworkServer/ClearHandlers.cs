﻿#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Clear all registered callback handlers.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.ClearHandlers.html")]
	[System.Serializable]
	public class ClearHandlers : StateAction {

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkServer.Reset ();
			Finish ();
		}
	}
}
#endif