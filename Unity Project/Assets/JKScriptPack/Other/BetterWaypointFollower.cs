/*
 *================================================================================
 *
 *  BetterWaypointFollower.cs
 *
 *================================================================================
 *
 *	Makes a GameObject follow a path marked out by waypoints.  The GameObject will
 *	chase a specified object if it comes within range (and it can see the object).
 *
 *	Attach this script to a GameObject.  Then drag other GameObjects into
 *	the waypoint list.
 *
 *	The GameObject will move from waypoint to waypoint, in order.
 *
 *	WalkingSpeed	in metres per second.
 *	TurningSpeed	in degrees per second.
 *	RepeatForever	repeat the path forever.
 *	PingPong		at the end, go backwards through the waypoints.
 *  ChaseObject		the specified object will be chased...
 *  ChaseRange		...if it comes within this range.
 *  ChaseAngle		...and angle of view.
 *
 *	v1.13 -- Re-written in C#; changed chase to use cone-of-view.
 *	v1.14 -- Added a couple of features:
 *				hudAlert: enables object (on GUI) which appears when being chased.
 *				min Closeness: stops chasing when it gets within this distance of the target.
 *	v1.23 -- Added ability to change animations
 *
 *	TO DO:
 *	- Fix jerkiness when dealing with a kinematic RigidBody
 *	- Include the waypoint's child objects in the list, too.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterWaypointFollower : MonoBehaviour {

	public GameObject[] waypointList = new GameObject[1];
	public float walkingSpeed = 4;			// distance per second
	public float turningSpeed = 90;			// degrees per second
	public bool repeatForever = true;
	public bool pingPong = false;			// reverse path at end?
	public bool glide = false;				// do we turn and walk at the same time?

	private float autocorrectDistance = 0.1f;
	private float autocorrectAngle = 1.0f;

	private GameObject target;
	private int index = 0;
	private int increment = 1;				// Switches to -1 if reversing

	[System.Serializable]
	public class Chase {
		public GameObject chaseObject;
		public float range = 10.0f;
		public float angle = 90;
		public float minCloseness = 2.0f;
		public GameObject hudAlert;
	}
	public Chase chase;

	public string idleAnimation;
	public string walkAnimation;
	public bool idleWhenTurning = false;
	private Animation anim;

	void Start () {
		if (chase.hudAlert) chase.hudAlert.SetActive(false);
		anim = GetComponentInChildren<Animation>();
		if (anim) anim.Play(idleAnimation, PlayMode.StopAll);
	}
	
	void Update () {

		// Where am I?
		Vector3 currentPos = this.transform.position;
		Vector3 currentHeading = this.transform.forward;
		
		// Do we need to chase the victim?
		target = CurrentWaypoint();
		if (chase.chaseObject) {
			Vector3 victimPos = chase.chaseObject.transform.position;
			Vector3 victimHeading = victimPos - currentPos;
//			if(gaze) gaze.transform.rotation = Quaternion.LookRotation(victimHeading);
			float victimDistance = Vector3.Distance(currentPos, victimPos);
			float victimAngle = Vector3.Angle(currentHeading, victimHeading);
			if (victimDistance <= chase.range && victimAngle <= (chase.angle/2)					// Check range;
					&& !Physics.Raycast(currentPos, victimHeading, victimDistance - 1.5f)) {	// Does the ray collide with anything along the way?
				target = chase.chaseObject;
			}
			if (chase.hudAlert) chase.hudAlert.SetActive(target == chase.chaseObject);
		}

		// If we have a target... (will be null if the waypointlist is empty and we're not chasing a victim)
		if (target) {

			// Where is it?
			Vector3 targetPos = new Vector3(target.transform.position.x, currentPos.y, target.transform.position.z);
			Vector3 targetHeading = targetPos - currentPos;

			// Have we reached the target?
			if (targetHeading.magnitude < autocorrectDistance) {

				// Move on to next target
				transform.position = targetPos;
				if (target != chase.chaseObject) {
					target = NextWaypoint();
				}

			} else {

				// Do we need to turn?
				if (Vector3.Angle(currentHeading, targetHeading) > autocorrectAngle) {

					// Turn towards the target
					Vector3 newDirection = Vector3.RotateTowards(currentHeading, targetHeading, turningSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
					transform.rotation = Quaternion.LookRotation(newDirection);

					if (idleWhenTurning) AnimateIdle();

				} else {

					// Face the target
					transform.rotation = Quaternion.LookRotation(targetHeading);

				}

				// If we're facing the object (or if glide is enabled) then walk
				if (glide || Vector3.Angle(currentHeading, targetHeading) < autocorrectAngle) {

					if (chase.chaseObject && target == chase.chaseObject && targetHeading.magnitude <= chase.minCloseness) {
						// Do nothing
						AnimateIdle();
					} else {
						
						// Walk toward the target
						transform.position = Vector3.MoveTowards(currentPos, targetPos, walkingSpeed * Time.deltaTime);
						AnimateWalk();

					}
				}

			}
		}

	}

	private void AnimateIdle() {
		if (anim && !anim.IsPlaying(idleAnimation)) {
			anim.CrossFade(idleAnimation);
		}
	}

	private void AnimateWalk() {
		if (anim && !anim.IsPlaying(walkAnimation)) {
			anim.CrossFade(walkAnimation);
		}
	}

	private GameObject CurrentWaypoint() {
		if (waypointList.Length <= 0) return null;
		if (index < 0) index = 0;
		if (index >= waypointList.Length) index = waypointList.Length - 1;
		return (waypointList[index]);
	}

	private GameObject NextWaypoint() {
		if (waypointList.Length <= 0) return null;
		index += increment;
		if (index >= waypointList.Length) {
			if (pingPong) {
				increment = -1;
				index = waypointList.Length - 1;
			} else if (repeatForever) {
				index = 0;
			} else {
				index = waypointList.Length - 1;
				increment = 0;
			}
		}
		if (index < 0) {
			if (repeatForever) {
				increment = 1;
			} else {
				increment = 0;
			}
			index = 0;		
		}
		return waypointList[index];
	}


}

