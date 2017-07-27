using UnityEngine;
using System.Collections;
using System;

namespace ICode{
	public class OnLevelWasLoadedHandler : MonoBehaviour {
		public Action<int> onLevelWasLoaded;
		#if UNITY_5_4
		private void Awake(){
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) => {
				if (onLevelWasLoaded != null) {
					onLevelWasLoaded(scene.buildIndex);			
				}
			};
			
		}
		#else
		private void OnLevelWasLoaded(int level) {
			if (onLevelWasLoaded != null) {
				onLevelWasLoaded(level);			
			}
		}
		#endif
	}
}