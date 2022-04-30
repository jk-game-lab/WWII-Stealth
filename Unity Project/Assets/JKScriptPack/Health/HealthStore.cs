/*
 *	HealthStore.cs
 * 
 *	This script keeps track of health across multiple scenes.
 *	
 *	Do NOT put this script into your game hierarchy.
 *	It should remain in the Assets.
 *
 *	v1.19 -- added to JKScriptPack
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthStore : object {

	public static float health = 1.0f;

	public static void Add(float percentage) {
		health += percentage;
		if (health >= 1.0f) {
			health = 1.0f;
		} else if (health <= 0.0f) {
			health = 0.0f;
		}
	}
	
	public static void Subtract(float percentage) {
		Add(-percentage);
	}
	
}
