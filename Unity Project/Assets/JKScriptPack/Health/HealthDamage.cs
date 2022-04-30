/*
 *	HealthDamage.cs
 * 
 *	Attach this script to the bullet.
 *	This script works in tandem with Health.cs
 *
 *	(Note: for some reason this only works for bullet detecting FPC; does not work for FPC detecting bullet!)
 *
 *	NOTE: if using this script, your Assets folder MUST contain
 *	the HealthStore script.
 *
 *	v1.15 -- added to JKScriptPack
 *	v1.19 -- updated to use HealthStore.
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDamage : MonoBehaviour {

	public GameObject weapon;
	public float damage = -0.2f;

	void Start () {
	}

	void OnCollisionEnter(Collision other) {
		//if (other.gameObject.name != "Terrain") Debug.Log("OnCollision: " + other.gameObject.name);
		if (weapon && (other.gameObject.name == weapon.name || other.gameObject.name == weapon.name + "(Clone)")) {
			HealthStore.Add(damage);
			//Destroy(other.gameObject);
			Destroy(this.gameObject);
		}
	}

	void OnControllerColliderHit(ControllerColliderHit other) {
		//if (other.gameObject.name != "Terrain") Debug.Log("OnControllerColliderHit: " + other.gameObject.name);
		if (weapon && (other.gameObject.name == weapon.name || other.gameObject.name == weapon.name + "(Clone)")) {
			HealthStore.Add(damage);
			Destroy(other.gameObject);
			//Destroy(this.gameObject);
		}
	}

}
