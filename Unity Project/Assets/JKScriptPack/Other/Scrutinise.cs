/*
 *	Scrutinise.cs
 *
 *	Apply this script to the Main Camera on the First Person Controller.
 * 
 */

using UnityEngine;
using System.Collections;

public class Scrutinise : MonoBehaviour {

	public KeyCode zoomKey = KeyCode.None;
	public float zoomedFOV = 20.0f;
	public float transitionTime = 0.33f;

	private float defaultFOV = 60.0f;
	private bool zoomed = false;
	private float ratio = 0.0f;

	void Start () {
		defaultFOV = this.GetComponent<Camera>().fieldOfView;
	}
	
	void Update () {
	
		// Check for keypress
		zoomed = Input.GetKey(zoomKey);

		// Animate accordingly
		if (zoomed) {
			if (ratio < transitionTime) {
				ratio += Time.deltaTime;
				if (ratio > 1.0f) {
					ratio = 1.0f;
				}
			}
		} else {
			if (ratio > 0.0f) {
				ratio -= Time.deltaTime;
				if (ratio < 0.0f) {
					ratio = 0.0f;
				}
			}
		}
		this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(defaultFOV, zoomedFOV, ratio);

	}

}
