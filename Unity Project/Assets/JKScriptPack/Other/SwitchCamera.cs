/*
 *	SwitchCamera.cs 
 * 
 * 	Switches between specified cameras.  Set the first item to
 * 	be the main camera, e.g. FirstPersonCharacter.
 * 
 * 	Attach this script to any object in the game.
 * 	Do not attach it to the camera, because it may diable itself!
 * 
 * 	v1.24 -- added to JKScriptPack
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchCamera : MonoBehaviour {

	[System.Serializable]
	public class CameraCombo {
		public GameObject camera;
		public KeyCode key = KeyCode.None;
	}
	public List<CameraCombo> combos;
	public bool temporary = true;

	void Reset () {
		CameraCombo main = new CameraCombo();
		main.camera = GameObject.FindGameObjectWithTag("MainCamera");
		combos = new List<CameraCombo>();
		combos.Add(main);
	}

	void Start () {
		if (combos.Count > 0) {
			enableCombo(combos[0]);
		}
	}

	void Update () {
		foreach (CameraCombo combo in combos) {
			if (Input.GetKeyDown(combo.key)) {
				enableCombo(combo);
				break;
			}
			if (temporary && Input.GetKeyUp(combo.key)) {
				enableCombo(combos[0]);
				break;
			}
		}				
	}

	void enableCombo (CameraCombo choice) {
		foreach (CameraCombo combo in combos) {
			combo.camera.SetActive(combo == choice);
		}
	}

}
