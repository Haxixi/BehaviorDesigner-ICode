#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Unloads all GameObjects associated with the given scene.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.UnloadScene.html")]
	[System.Serializable]
	public class UnloadScene : StateAction {
		[NotRequired]
		[Tooltip("Index of the scene to unload.")]
		public FsmInt sceneIndex;
		[NotRequired]
		[Tooltip("Name of the scene to unload.")]
		public FsmString sceneName;

		public override void OnEnter ()
		{
			base.OnEnter ();
			if (!sceneName.IsNone) {
				SceneManager.UnloadScene (sceneName.Value);
			} else if (!sceneIndex.IsNone) {
				SceneManager.UnloadScene (sceneIndex.Value);
			} 
			Finish ();
		}
	}
}
#endif