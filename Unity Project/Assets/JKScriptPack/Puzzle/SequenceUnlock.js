/*
 *	SequenceUnlock.js
 * 
 *	User must click (with the mouse) a set of objects in the correct order
 *	to unlock a barrier.
 *
 *	The item list specifies the items that can be clicked.
 *	The sequence length indicates how many of these are needed to unlock
 *	the barrier.  The order of the sequence follows the item order in the list.
 *
 *	Add this script to the first person controller.
 *	The 'If Clickable Show' gameobject should ideally be linked to a crosshair object on the UI.
 *
 *	v1.00 -- Added to JKScriptPack.
 *	v1.12 -- improved version, highlighing every clicked object.
 *	v1.20 -- bugfix: JS code was throwing up some warnings.
 *	v1.24 -- updated to avoid errors if main camera is changed.
 *
 */

public var itemList : GameObject[];
public var sequenceLength : int = 0;

public var highlightDuration : float = 0.25f;
public var ifClickableShow : GameObject;

public var clickColour : Color = Color.yellow;
public var clickSound : AudioClip;

public var unlockColour : Color = Color.green;
public var unlockSound : AudioClip;
public var unlockDisable : GameObject;
public var unlockEnable : GameObject;

public var allowRelock : boolean = true;

class ItemInfo {
	var countdown : float;
	var material : Material;
	var originalShader : Shader;
	var originalColor : Color;
	var clickedShader : Shader;
}
private var itemInfo : ItemInfo[];		// Note: stored parallel to itemList[] to avoid Inspector layout issues.
private var correctSoFar : int = 0;
private var detectionDistance : float = 20.0f;
private var a : AudioSource;
																																														
function Start () {

	// Initialise all item info
	itemInfo = new ItemInfo[itemList.length];
	for( var i = 0; i < itemList.length; i++ ){

		// Create a new record
		itemInfo[i] = new ItemInfo();
		itemInfo[i].countdown = 0.0f;
		if (itemList[i]) {
			itemInfo[i].material = itemList[i].GetComponent(Renderer).material;
			itemInfo[i].originalShader = itemInfo[i].material.shader;
			itemInfo[i].originalColor = itemInfo[i].material.color;
			itemInfo[i].clickedShader = Shader.Find("Self-Illumin/Diffuse");
		}

	}
	if (sequenceLength <= 0 || sequenceLength > itemList.length) {
		sequenceLength = itemList.length;
	}

	// Reset objects
	if (ifClickableShow) ifClickableShow.SetActive(false);
	if (unlockEnable) unlockEnable.SetActive(false);
	if (unlockDisable) unlockDisable.SetActive(true);

	// Set up audio
	a = gameObject.AddComponent(AudioSource);

}

function Update () {

	// Check for mouse click
	var mouseClicked = false;
	if (Input.GetMouseButtonDown (0)) {
		mouseClicked = true;
	}

	// Work out which item falls within our gaze at the point on screen where our mouse clicked
	var mainCamera : Camera;
	if (GetComponent.<Camera>()) {
		mainCamera = GetComponent.<Camera>();
	} else {
		mainCamera = Camera.main;
	}
	if (mainCamera) {
		var ray : Ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		var hit : RaycastHit;
		if (Physics.Raycast(ray, hit, detectionDistance)) {

			// Check to see if it's in the list
			var isClickable = false;
			for (var i = 0; i < itemList.length; i++) {
				if (hit.collider.gameObject == itemList[i]) {
					isClickable = true;
					if (mouseClicked) {
						Highlight(i, clickColour);
						a.PlayOneShot(clickSound);
					}
				}
			}
			if (ifClickableShow) ifClickableShow.SetActive(isClickable);

			// Check if it's the next item in the sequence
			if (mouseClicked) {
				if (hit.collider.gameObject == itemList[correctSoFar]) {
					correctSoFar++;
					if (correctSoFar >= sequenceLength) {
						Unlock();
					}
				} else {
					correctSoFar = 0;
				}
			}

		}
	}

	// Count down any highlighter timers
	for (i = 0; i < itemList.length; i++) {
		if (itemInfo[i].countdown > 0.0f) {
			itemInfo[i].countdown -= Time.deltaTime;
			if (itemInfo[i].countdown <= 0.0f) {
				Unhighlight(i);
			}
		}
	}		

}

function Highlight(i : int, c : Color) {
	if (itemList[i]) {
		itemInfo[i].countdown = highlightDuration;
		itemInfo[i].material.shader = itemInfo[i].clickedShader;
		itemInfo[i].material.color = c;
	}
}

function Unhighlight(i : int) {
	if (itemList[i]) {
		itemInfo[i].countdown = 0.0f;
		itemInfo[i].material.shader = itemInfo[i].originalShader;
		itemInfo[i].material.color = itemInfo[i].originalColor;
	}
}

function Unlock() {

	// Highlight all correct items
	for (var i = 0; i < correctSoFar; i++) {
		Highlight(i, unlockColour);
	}
	a.PlayOneShot(unlockSound);

	// Flip state -- allows the user to re-lock with sequence
	if (allowRelock) {
		if (unlockEnable) unlockEnable.SetActive(!unlockEnable.activeInHierarchy);
		if (unlockDisable) unlockDisable.SetActive(!unlockDisable.activeInHierarchy);
	} else {
		if (unlockEnable) unlockEnable.SetActive(true);
		if (unlockDisable) unlockDisable.SetActive(false);
	}
	if (ifClickableShow) ifClickableShow.SetActive(false);

	// Reset lock
	correctSoFar = 0;

}