/*
 *	Oscillate.cs
 *
 *	Makes an object move up-and-down by a set distance.
 *
 *	Attach this script to any gameobject.
 *
 *	v1.09 -- now uses localRotation (i.e. rotation relative to parent)
 *
 */

using UnityEngine;
using System.Collections;

public class Oscillate : MonoBehaviour {

	public float travelTime = 1;
	public Vector3 distance = new Vector3(0, 1, 0);
	public bool smooth = true;

	private Vector3 origin;
	private float elapsedTime;
	private bool reverse;

	void Start () {
		origin = transform.localPosition;
		reverse = false;
	}

	void Update () {

		if (travelTime >= 0) {

			// Where is the target?
			Vector3 target = origin + distance;

			// Are we there yet?
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= travelTime) {
				elapsedTime = 0;
				reverse = !reverse;
			}

			// Move the object
			float proportion = elapsedTime / travelTime;
			if (smooth) {
				proportion = 0.5f - Mathf.Cos (proportion * 180.0f * Mathf.Deg2Rad) / 2;
			}
			if (reverse) {
				transform.localPosition = Vector3.Lerp (target, origin, proportion);
			} else {
				transform.localPosition = Vector3.Lerp (origin, target, proportion);
			}

		}

	}

}
