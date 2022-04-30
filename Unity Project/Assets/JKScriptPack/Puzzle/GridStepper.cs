using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// needed for List<>

public class GridStepper : MonoBehaviour {

	public float stepSize = 1.0f;
	public float stepTime = 0.5f;
	public bool glide = false;

	private List<Vector3> queue;
	private float elapsedTime;
	private Vector3 prevPos;
	private Vector3 nextPos;

	void Start () {
		queue = new List<Vector3>();
		elapsedTime = 0;
		prevPos = transform.position;
		nextPos = transform.position;
	}
	
	void Update () {
	
		// check if a key has been pressed
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			queue.Add(Vector3.forward);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow)) {
			queue.Add(Vector3.back);
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			queue.Add(Vector3.left);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)) {
			queue.Add(Vector3.right);
		}

		// Is there a queue?
		if (queue.Count > 0) {

			// Are we starting a new step?
			if (elapsedTime == 0) {
				prevPos = transform.position;
				nextPos = prevPos + queue[0] * stepSize;
			}

			elapsedTime += Time.deltaTime;

			// Are we partway through a step?
			if (elapsedTime < stepTime) {

				// Move proportionately
				float proportion = elapsedTime / stepTime;
				if (!glide) {
					proportion = 0.5f - Mathf.Cos (proportion * 180.0f * Mathf.Deg2Rad) / 2;
				}
				transform.position = Vector3.Lerp (prevPos, nextPos, proportion);

			// Have we reached our destination?
			} else {

				// Move to destination
				transform.position = nextPos;
				queue.RemoveAt(0);
				elapsedTime = 0;

			}

		}

	}

	void OnCollisionEnter (Collision other) {
		Debug.Log("Collision!");
		Collided ();
	}

	void OnTriggerEnter (Collider other) {
		Debug.Log("Trigger!");
		Collided ();
	}

	private void Collided() {

		// If we've hit something, cancel the current step
		transform.position = prevPos;
		if (queue.Count > 0) {
			queue.RemoveAt(0);
			elapsedTime = 0;
		}
		
	}

}
