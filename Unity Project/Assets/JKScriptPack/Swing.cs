/*
 *	Swing.cs
 *
 *	Makes an object swing around its pivot.
 *
 *	Attach this script to a gameobject.
 *
 *	v1.09 -- now uses localRotation (i.e. rotation relative to parent)
 *	v1.25 -- bugfix; start & finish angles now relative to initial rotation.
 *
 */

using UnityEngine;
using System.Collections;

public class Swing : MonoBehaviour {

	public float speed = 2;
	public Vector3 axis = new Vector3(0, 0, 1);
	public float angle = 45;

	private Quaternion angleA;
	private Quaternion angleB;
	private float elapsedTime;

	void Start () {
		angleA = transform.localRotation * Quaternion.AngleAxis(angle, axis);
		angleB = transform.localRotation * Quaternion.AngleAxis(-angle, axis);
		elapsedTime = 0.0f;
	}

	void Update () {
		elapsedTime += Time.deltaTime;
		transform.localRotation = Quaternion.Lerp(angleA, angleB, 0.5f * (1.0f + Mathf.Sin(elapsedTime * speed)) );
	}


}
