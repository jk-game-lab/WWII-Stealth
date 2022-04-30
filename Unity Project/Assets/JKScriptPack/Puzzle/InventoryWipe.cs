/*
 *	InventoryWipe.cs
 * 
 *	Use in combination with the InventoryPrefab.
 *	
 *	This script should be applied to a gameobject in a 'game over',
 *	intro menu screen or in Level 1.  It will reset the inventory when
 *	the scene loads.
 *
 *	v1.24 -- added to JKScriptPack
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWipe : MonoBehaviour {

	private GameObject prefab;
	private Inventory script;

	void Start() {
		prefab = GameObject.Find("Inventory Prefab");
		if (prefab) {
			script = prefab.GetComponent<Inventory>();
			if (script) {
				script.ClearCollected();
			}
		}
	}

}
