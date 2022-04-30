/*
 * 	Collector.cs
 * 
 * 	Attach this to the first person controller (FPC).
 * 	Create a list of GameObjects to collect.  When the FPC
 * 	collides with the specified GameObject, the object will disappear
 * 	and be added to a list.
 * 
 * 	When you have collected all objects, your reward will be to reveal (or hide) 
 * 	another gameobject (such as a barrier or prize).
 * 
 * 	Collectable gameobjects do not need to be set as triggers; this will be done
 * 	automatically.
 * 
 * 	v1.00 -- added to JKScriptPack
 * 	v1.06 -- Added key-to-pickup
 *	v1.09 -- Amended to work with Unity 2017
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collector : MonoBehaviour {

	[System.Serializable]
	public class Collectable {
		public GameObject gameObject;
		public bool collected = false;
		//public int points = 10;
	}
	public Collectable[] collectables;

	public KeyCode pressKeyToCollect = KeyCode.None;
	public AudioClip collectionSound;
	public AudioClip finishedSound;
	public GameObject finishedReveal;
	public GameObject finishedHide;

	private AudioSource audiosource;

	void Start () {

		// Reset rewards
		if (finishedReveal) {
			finishedReveal.gameObject.SetActive(false);
		}
		if (finishedHide) {
			finishedHide.gameObject.SetActive(true);
		}

		// If we're using keypress to pick up, make all collectables be triggers
		SetAllTriggers (pressKeyToCollect != KeyCode.None);

		// Initialise audio
		audiosource = gameObject.AddComponent<AudioSource>();

	}

	private void SetAllTriggers(bool state) {
		foreach (Collectable item in collectables) {
			if (item.gameObject && item.gameObject.GetComponent<Collider>()) {
				item.gameObject.GetComponent<Collider>().isTrigger = state;
			}
		}
	}

	void OnControllerColliderHit(ControllerColliderHit other) {		// Use for collection without a keypress
		if (other.gameObject.name != "Terrain") {					// Don't waste time if the FPC is colliding with the terrain
			//Debug.Log("OnControllerColliderHit=" + other.gameObject.name);
			CollectItem (other.gameObject);
		}
	}

	void OnTriggerStay(Collider other) {							// Use for collection with a keypress
		//Debug.Log("OnTriggerStay=" + other.gameObject.name);
		if (Input.GetKeyDown(pressKeyToCollect)) {
			CollectItem (other.gameObject);
		}
	}

	//void OnCollision(Collision other) {
	//	Debug.Log("OnCollision=" + other.gameObject.name);
	//}

	private void CollectItem(GameObject other) {

		// Is the item in our list?
		foreach (Collectable collectable in collectables) {
			if (other.Equals(collectable.gameObject)) {

				// Collect it
				collectable.collected = true;
				Destroy(other);
				audiosource.PlayOneShot(collectionSound);

				// Have all objects been collected?
				if (AllCollected()) {
					if (finishedReveal) {
						finishedReveal.gameObject.SetActive(true);
					}
					if (finishedHide) {
						finishedHide.gameObject.SetActive(false);
					}
					audiosource.PlayOneShot(finishedSound);
				}

				break;
			}
		}
	}

	public bool AllCollected() {
		bool all = true;
		foreach (Collectable item in collectables) {
			if (!item.collected) {
				all = false;
				break;
			}
		}
		return all;
	}

}
