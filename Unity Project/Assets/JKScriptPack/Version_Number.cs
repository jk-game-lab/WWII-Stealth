/*
 *================================================================================
 *  Version_Number.cs
 *================================================================================
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Version_Number : MonoBehaviour {

	void Start () {
		Debug.Log("JKScriptPack version 1.25");
	}

	/*
	 *	v1.25
	 *		- Swing (bugfix)
	 *
	 *	v1.24
	 *		- Inventory (basic inventory system that persists across scenes)
	 *		- SwitchCamera
	 *		- SequenceUnlock (bugfix)
	 *
	 *	v1.23
	 *		- BetterWaypointFollower (added animation control)
	 *
	 *	v1.22
	 *		- SceneCheat (use a key to jump to a level)
	 *
	 *	v1.21
	 *		- DoorTwoWay (quick-and-nasty two-way door)
	 *
	 *	v1.20
	 *		- SequenceUnlock (bugfix)
	 *
	 *	v1.19
	 *		- Health now persists across multiple scenes.
	 *
	 *	v1.18
	 *		- Countdown (counts down to end of game)
	 *		- HealthChange (updated with 'player only' option)
	 *
	 *	v1.17
	 *		- Battery (new -- allows a battery to be charged / discharged)
	 *		- DoorSwing (now allows the handle to turn)
	 *		- Sniper (now automatically creates bullets if none specified)
	 *
	 *	v1.16
	 *		- LoadNewScene (now allows a combination of trigger & keypress)
	 *
	 *	v1.15
	 *		- HealthDamage (when getting shot by a bullet)
	 *
	 * 	v1.14
	 *  	- BetterWaypointFollower (now enables HUD 'alert' object when being chased; also minimum closeness when chasing)
	 *	v1.13
	 *		- BetterWaypointFollower (now with cone of vision and will not chase through walls)
	 *		- WaypointTreacle (slows enemy down in a trigger zone)
	 *		- Sniper (automatically shoots within a cone of vision)
	 *		- DoorSwing (bugfix)
	 *
	 *	v1.12
	 *		- SequenceUnlock (improved to suit keypad entry system)
	 *
	 *	v1.11
	 *		- HealthChange (improved with continuous drain option)
	 *		- Flicker (new -- makes light flicker)
	 *		- DoorSwing (updated to allow keypress to shut door)
	 *		- BetterOnTriggerReveal (improved replacement for OnTriggerReveal)
	 *
	 *	v1.10
	 *		- Health & HealthChange (control health bar on UI)
	 *
	 *	v1.09
	 *		- Collector (updated to work correctly on Unity 2017)
	 *		- LoadNewScene (updated to work correctly on Unity 2017)
	 *		- Oscillate & Swing (fixed orientation bug)
	 *
	 *	Older versions not listed.
	 */

}
