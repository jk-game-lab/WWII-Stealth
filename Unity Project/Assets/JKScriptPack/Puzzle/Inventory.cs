/*
 *	Inventory.cs
 * 
 *	This script keeps track of items collected across multiple scenes.
 *	
 *	Do not drag this script into your game.  Instead, drop the 
 *	associated PREFAB into the ROOT of any scene hierarchy.
 *
 *	We cannot attach gameobjects directly from different scenes;
 *	therefore you must specify the list of collectable gameobjects
 *	using the object's NAME as it appears in the hierarchy.
 *
 *	The Inventory Prefab contains its own UI canvas.  If you wish to
 *	have an indicator to show that the object has been collected, add 
 *	an relevant object to the Inventory Prefab canvas (e.g. a sprite
 *	image).  This will then need to be linked to the collectable as
 *	its 'Hud Object'.
 *
 *	v1.24 -- added to JKScriptPack
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	//--------------------------------------------------------------------------------
	// Collector script auto-added to the FPC
	public class InventoryCollector : MonoBehaviour {

		private GameObject prefab;
		private Inventory script;

		void Start() {
			prefab = GameObject.Find("Inventory Prefab");
			if (prefab) {
				script = prefab.GetComponent<Inventory>();
			}
		}

		void OnTriggerEnter(Collider other) {
			if (script) {
				script.Collect(other.gameObject.name);
			}
		}

	}
	//--------------------------------------------------------------------------------

	[System.Serializable]
	public class Collectable {
		public string objectName;
		public bool collected = false;
		public GameObject hudObject;
		public string ifCollectedShow;
		public string ifCollectedHide;
	}
	public List<Collectable> collectables;

	public KeyCode pressKeyToCollect = KeyCode.None;
	public AudioClip collectionSound;
	public AudioClip allCollectedSound;
	public string allCollectedShow;
	public string allCollectedHide;
	
	private AudioSource audiosource;

	//--------------------------------------------------------------------------------
	// If this scene is reloaded, make sure it does not re-create this gameobject.
	public static Inventory existingInstance;
	void Awake () {
		if (!existingInstance) {
			existingInstance = this;
		} else if(existingInstance != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(transform.gameObject);
	}
	//--------------------------------------------------------------------------------

	void Reset () {
		collectables = new List<Collectable>();
	}

	void Start () {

		// Initialise audio
		audiosource = gameObject.AddComponent<AudioSource>();
	}

	// How do we detect a new scene?

	void Update() {

		//
		//	Scans continually in case we've loaded a new scene
		//	This is helpful because it also allows us to react 
		//	to runtime Inspector changes when testing a game.
		//

		GameObject[] gameobjects = Resources.FindObjectsOfTypeAll<GameObject>();	// Gameobject.Find( can't find inactive objects

		// Scan for FPC and add an inventory collector if needed
		GameObject fpc = GameObject.Find("FPSController");
		if (fpc) {
			if (!fpc.GetComponent<InventoryCollector>()) {
				fpc.AddComponent<InventoryCollector>();
			}
		}

		// Scan (and update) collectable gameobjects
		foreach (Collectable item in collectables) {

			// Update visitibility of all live objects in this scene
			GameObject obj = GameObject.Find(item.objectName);
			if (obj) {

				// If it has already been collected, destroy it
				if (item.collected) {
					Destroy (obj);
				}

				// Otherwise, force its trigger setting
				else {
					Collider c = obj.gameObject.GetComponent<Collider>();
					if (c) c.isTrigger = true;
				}

			}

			// Update visibility of HUD objects
			if (item.hudObject) {
				item.hudObject.SetActive(item.collected);
			}

			// Update visibility of linked objects, if found in this scene
			foreach (GameObject g in gameobjects) {
				if (g.name == item.ifCollectedShow) g.SetActive(item.collected);
				if (g.name == item.ifCollectedHide) g.SetActive(!item.collected);
			}

		}

		// Check if all objects have been collected
		bool all = AllCollected();
		foreach (GameObject g in gameobjects) {
			if (g.name == allCollectedShow) g.SetActive(all);
			if (g.name == allCollectedHide) g.SetActive(!all);
		}

	}

	public void Collect(string name) {
		foreach (Collectable item in collectables) {
			if (item.objectName == name) {
				item.collected = true;
				if (AllCollected()) {
					audiosource.PlayOneShot(allCollectedSound);
				} else {
					audiosource.PlayOneShot(collectionSound);
				}
			}
		}
	}

	public void ClearCollected() {
		foreach (Collectable item in collectables) {
			item.collected = false;
		}
	}

	private bool AllCollected() {
		bool all = true;
		foreach (Collectable item in collectables) {
			if (!item.collected) {
				all = false;
				break;
			}
		}
		return all;
	}

	// We can even make this detect the game over scene and auto-destoy itself, to hide the inventory & start afresh next time
	//https://answers.unity.com/questions/1229370/destroy-object-with-dontdestroyonload.html
	// Or maybe better to have a WipeInventory script that can be applied to chosen scenes

}
