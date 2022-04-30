/*
 *	Revolve.cs
 *
 *	Makes an object revolve around its pivot point.
 *
 *	Attach this script to any gameobject.
 *
 */

using UnityEngine;
using System.Collections;

public class Revolve : MonoBehaviour {

	public float speed = 1;
	public Vector3 axis = new Vector3(0, 1, 0);

	void Start () {
	
	}
	
	void Update () {
		transform.Rotate(axis, speed * 360 * Time.deltaTime);
	}

}
