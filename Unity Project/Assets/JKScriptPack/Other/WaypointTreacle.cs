/*
 *================================================================================
 *
 *  WaypointTreacle.cs
 *
 *================================================================================
 *
 *	If a BetterWaypoint-controlled object moves into this trigger zone then its 
 *	walking speed will decrease (can also increase).  This can be set to last a 
 *	specified period of time.
 *
 *	Ensure the waypoint-controlled object has:
 *		- a collider (on the same object that contains the waypoint script)
 *		- a rigidbody with gravity off and isKinematic ticked.
 *
 *	Attach this script to a trigger zone.
 *	If timeout is set to 0 then the speed slows within the trigger zone only.
 *
 *	v1.13 -- added to JKScriptPack.
 *
 *	TO DO:
 *	- Detect multiple items, storing them temporarily in a list.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTreacle : MonoBehaviour {

	public float speedMultiplier = 0.3f;
	public float timeout = 0;

	private GameObject victim;
	private float originalSpeed;
	private float countdown;

	void Start () {
		
		// If it's not already a trigger, force it to be
		if (this.GetComponent<Collider>()) {
			this.GetComponent<Collider>().isTrigger = true;
		}

	}
	
	void Update () {
		if (countdown > 0) {
			countdown -= Time.deltaTime;
			if (countdown <= 0) {
				countdown = 0;
				RestoreSpeed(victim);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		victim = other.gameObject;
		SlowSpeed(victim);
	}

	void OnTriggerExit(Collider other) {
		if (timeout == 0) {
			RestoreSpeed(victim);
		}
	}

	private void SlowSpeed(GameObject v) {

		BetterWaypointFollower BWFsettings = v.GetComponent<BetterWaypointFollower>();
		if (BWFsettings) {
			originalSpeed = BWFsettings.walkingSpeed;
			BWFsettings.walkingSpeed *= speedMultiplier;
			countdown = timeout;
		}

	}

	private void RestoreSpeed(GameObject v) {

		BetterWaypointFollower BWFsettings = v.GetComponent<BetterWaypointFollower>();
		if (BWFsettings) {
			BWFsettings.walkingSpeed = originalSpeed;
		}

	}

}
