/*
 *	SceneCheat.cs
 * 
 *	This script lets the player jump to any scene with a keypress.
 *	It is intended as an aid for game developers and should be
 *	disabled in the final build.
 *
 *	Do not drag this script into your game.  Instead, drop the 
 *	associated PREFAB into the ROOT of any scene hierarchy.
 *
 *	v1.22 -- added to JKScriptPack
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCheat : MonoBehaviour {

	public KeyCode exitKey = KeyCode.None;

	[System.Serializable]
	public class CheatItem {
		public KeyCode secretKey = KeyCode.None;
		public string sceneName;
	}
	public List<CheatItem> cheatList;

	//--------------------------------------------------------------------------------
	// If this scene is reloaded, make sure it does not re-create this gameobject.
	public static SceneCheat existingInstance;
	void Awake () {

		if (!existingInstance) {
			existingInstance = this;
		} else if(existingInstance != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(transform.gameObject);
	}
	//--------------------------------------------------------------------------------

	void Reset () {

		// If empty, auto-populate the list
		if (SceneManager.sceneCountInBuildSettings > 0) {
			cheatList = new List<CheatItem>();
			for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
				CheatItem c = new CheatItem();
				c.sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));	// Unfortunately GetSceneByBuildIndex(i).name does not work for unloaded scenes due to a bug in Unity.
				if (i < 12) {
					// Unfortunately, due to KeyCode being enumerated, this appears to be the only way to do this.
					switch (i) {
						case 0: c.secretKey = KeyCode.F1; break;
						case 1: c.secretKey = KeyCode.F2; break;
						case 2: c.secretKey = KeyCode.F3; break;
						case 3: c.secretKey = KeyCode.F4; break;
						case 4: c.secretKey = KeyCode.F5; break;
						case 5: c.secretKey = KeyCode.F6; break;
						case 6: c.secretKey = KeyCode.F7; break;
						case 7: c.secretKey = KeyCode.F8; break;
						case 8: c.secretKey = KeyCode.F9; break;
						case 9: c.secretKey = KeyCode.F10; break;
						case 10: c.secretKey = KeyCode.F11; break;
						case 11: c.secretKey = KeyCode.F12; break;
					}
				}
				cheatList.Add(c);
			}
		}

	}

	void Start () {
	}

	private void Update() {

		// Is the key in our list?
		if (Input.GetKeyDown(exitKey)) {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
		foreach (CheatItem cheat in cheatList) {
			if (Input.GetKeyDown(cheat.secretKey)) {
				LoadScene(cheat.sceneName);
				break;
			}
		}

	}
	
	public void LoadScene(string sceneName) {
		if (sceneName.Equals("")) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		} else {
			SceneManager.LoadScene(sceneName);
		}
	}

}
