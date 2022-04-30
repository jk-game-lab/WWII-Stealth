/*
 *	ShrinkGrowWithJump.js
 *
 *	Note: has to be in JS (not CS) because CharacterMotor is not accessible from CS.
 *
 *	version 1.0 -- rough version controlled by keys
 *	version 1.1 -- jump added
 *	version 1.2 -- improved version with trigger zones
 *	version 1.3 -- added multiple trigger zones and sounds
 *	version 1.4 -- added transition animation
 */

/*

#pragma strict

public var sizeScale : float = 0.5f;
public var jumpScale : float = 0.5f;
public var transitionTime : float = 1.0f;

public var shrinkTriggers : GameObject[];
public var shrinkKey : KeyCode = KeyCode.None;
public var shrinkSound : AudioClip;

public var growTriggers : GameObject[];
public var growKey : KeyCode = KeyCode.None;
public var growSound : AudioClip;

private var shrunk : boolean;
private var originalHeight : float = 2.0f;
private var originalScale : Vector3;
private var originalJump : float = 1.0;
private var shrunkJump : float;
private var cm : CharacterMotor;
private var a : AudioSource;
private var transitionCountdown : float = 0.0f;

function Start () {
	shrunk = false;
	originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
	var cc : CharacterController = GetComponent(CharacterController);
	if (cc) {
		originalHeight = cc.height;
	}
	cm = GetComponent(CharacterMotor);
	if (cm) {
		originalJump = cm.jumping.baseHeight;
	}
	shrunkJump = originalJump * jumpScale;
	a = gameObject.AddComponent(AudioSource);
}

function Update () {

	// Check for keys
	if (Input.GetKeyDown(shrinkKey)) {
		shrink();
	} else if (Input.GetKeyDown(growKey)) {
		grow();
	}

	// If currently resizing
	if (transitionCountdown > 0.0f) {
	
		// Update size
		var ratio : float = (transitionCountdown / transitionTime);
		if (shrunk) {
			ratio = 1.0f - ratio;
		} else {


			// Work out how high we are above ground
//			var hit : RaycastHit;
//			var heightAboveGround : float = 0.0f;
//			if (Physics.Raycast (transform.position, transform.TransformDirection(Vector3.down) , hit, Mathf.Infinity)) {
//   			heightAboveGround = hit.distance;
//			}


			// Compensate for growth pushing us down
			var newScale : float = originalScale.y * Mathf.Lerp(1.0f, sizeScale, ratio);
			var scaleDelta : float = newScale - transform.localScale.y;
			var heightDelta : float = originalHeight * scaleDelta;
			transform.position += new Vector3(0.0f, heightDelta, 0.0f);

		}
		transform.localScale = originalScale * Mathf.Lerp(1.0f, sizeScale, ratio);
		if (cm) {
			cm.jumping.baseHeight = Mathf.Lerp(originalJump, shrunkJump, ratio);
			cm.jumping.extraHeight = Mathf.Lerp(originalJump, shrunkJump, ratio);
		}

		// Update time counters
		transitionCountdown -= Time.deltaTime;
		if (transitionCountdown < 0.0f) {
			transitionCountdown = 0.0f;
		}
	}
	
}

function OnCollisionEnter(other : Collision) {
	collidedWith(other.gameObject);
}

function OnControllerColliderHit(other : ControllerColliderHit) {
	collidedWith(other.gameObject);
}

function OnTriggerEnter(other : Collider) {
	collidedWith(other.gameObject);
}

function collidedWith(g : GameObject) {
	
	var match = false;
	for (var i = 0; i < shrinkTriggers.length; i++) {
		if (g == shrinkTriggers[i].gameObject) {
			match = true;
		}
	}
	if (match) {
		shrink();
	}
	
	match = false;
	for (i = 0; i < growTriggers.length; i++) {
		if (g == growTriggers[i].gameObject) {
			match = true;
		}
	}
	if (match) {
		grow();
	}

}

function shrink() {
	if (!shrunk) {
		shrunk = true;
		a.PlayOneShot(shrinkSound);
		transitionCountdown = transitionTime;
	}
}

function grow() {
	if (shrunk) {
		shrunk = false;
		a.PlayOneShot(growSound);
		transitionCountdown = transitionTime;
	}
}

*/