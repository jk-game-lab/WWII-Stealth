/*
 *	OnButtonExit.js
 * 
 *	When a button is pressed, exit the game.
 *
 *	Attach this script to a Button object.
 */
 
#pragma strict
import UnityEngine.UI;

public var key : KeyCode = KeyCode.None;

private var myButton : Button;

public function Awake() {
	myButton = gameObject.GetComponent(Button);
	myButton.onClick.AddListener(Exit);
}

public function Update() {
	if (Input.GetKeyDown(key)) {
		Exit();
	}
}

public function Exit() {
#if UNITY_EDITOR
	UnityEditor.EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
}

// Probably neeed to add this...
//public function Destroy() {
//	myButton.onClick.RemoveListener(Exit);
//}