/*
 *	Health.cs
 * 
 *	Attach this script to the health gameobject -- usually
 *	an image on the UI canvas.
 *
 *	Health = 0 for dead and 1 for fully alive.
 *
 *	NOTE: if using this script, your Assets folder MUST contain
 *	the HealthStore script.
 *
 *	v1.10 -- added to JKScriptPack
 *	v1.19 -- updated to use HealthStore.
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

	public string deadScene;
	public float dropPerSecond = 0.0f;

	private Vector3 originalScale;

	void Start () {
		if (HealthStore.health <= 0) {
			HealthStore.health = 1.0f;
		}
		originalScale = transform.localScale;
	}
	
	void Update () {

		// Drop over time
		HealthStore.Subtract(dropPerSecond * Time.deltaTime);

		// Update health bar
		this.transform.localScale = new Vector3(originalScale.x * HealthStore.health, originalScale.y, originalScale.z);

		// Check if we're dead
		if (HealthStore.health <= 0.0f) {
			if (deadScene.Equals("")) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			} else {
				SceneManager.LoadScene(deadScene);
			}
		}

	}

}
