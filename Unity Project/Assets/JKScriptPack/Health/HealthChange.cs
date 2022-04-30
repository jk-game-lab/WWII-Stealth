/*
 *	HealthChange.cs
 * 
 *	Attach this script to something bad or to a medical pack.
 *	This script works in tandem with Health.cs
 *
 *	Use a negative number to drain health; positive to add.
 *
 *	NOTE: if using this script, your Assets folder MUST contain
 *	the HealthStore script.
 *
 *	v1.10 -- added to JKScriptPack
 *	v1.11 -- added continuous detection
 *	v1.18 -- added "player only" option
 *	v1.19 -- updated to use HealthStore.
 *	v1.20 -- bugfix; now detects collider on child object.
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthChange : MonoBehaviour {

	public float changeBy = -0.1f;
	public bool destroyable = true;
	public bool continuous = false;
	public float changePerSecond = -0.2f;
	public bool playerOnly = true;

	private Collider mycollider;

	void Start () {
		mycollider = this.GetComponentInChildren<Collider>();
		if (mycollider) {
			mycollider.isTrigger = true;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		//Debug.Log ("Trigger: " + other.gameObject.name);
		if (ValidCollision(other.gameObject)) {
			HealthStore.Add(changeBy);
			if (destroyable) {
				Destroy(this.gameObject);
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (continuous && ValidCollision(other.gameObject)) {
			HealthStore.Add(changePerSecond * Time.deltaTime);
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.name != "Terrain") {
			//Debug.Log ("Collision: " + other.gameObject.name);
		}
	}

	bool ValidCollision(GameObject other) {
		return (!playerOnly || (playerOnly && (other.gameObject.tag == "Player" || other.gameObject.name == "FPSController")));
	}
}
