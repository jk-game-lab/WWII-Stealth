/*
 *================================================================================
 *
 *  WaypointFollower
 *
 *================================================================================
 *
 *	Makes a GameObject follow a path marked out by waypoints.
 *
 *	Attach this script to a GameObject.  Then drag other GameObjects into
 *	the waypoint list.  The GameObject will move from waypoint to waypoint,
 *	in order.
 *
 *	WalkingSpeed	in metres per second.
 *	TurningSpeed	in degrees per second.
 *	RepeatForever	repeat the path forever.
 *	PingPong		at the end, go backwards through the waypoints.
 *  ChaseObject		the specified object will be chased...
 *  ChaseRange		...if it comes within this range.
 *
 *--------------------------------------------------------------------------------
 *
 *  Version history:
 *
 *	v1.0	Original quickly-built version for Unity 3.
 *	v1.1	Adjusted to use GameObject array in the inspector.
 *	v2.0	Total re-write.
 *	v2.1	Added actions for when the chased is caught.
 *
 *  John King
 *
 *--------------------------------------------------------------------------------
 */

#pragma strict

// Waypoint traversal
public var waypointList : GameObject[];
private var index : int = 0;
public var repeatForever : boolean = true;
public var pingPong : boolean = false;
private var increment : int = 1;
private var target : GameObject;

// Movement
public var walkingSpeed : float = 4;				// distance per second
public var turningSpeed : float = 90;				// degrees per second
private var autocorrectAngle : float = 1;			// in degrees
private var autocorrectDistance : float = 0.1;
public var glide : boolean = false;					// do we turn and walk at the same time?

// Chase player
public var chaseObject : GameObject;
public var chaseRange : float = 10;
public var caughtSound : AudioClip;
private var a : AudioSource;
private var caught : boolean = false;
public var caughtAnimation : String;

// Debugging
private var DEBUGGING : boolean = false;
private var DebugMessage : String;

function Start() {
	a = gameObject.AddComponent(AudioSource);
}

function Update() {

	// Where am I?
	var currentPos : Vector3 = this.transform.position;
	var currentHeading : Vector3 = this.transform.forward;

	// Do we need to chase the player?
	if (chaseObject && Vector3.Distance(currentPos, chaseObject.transform.position) <= chaseRange) {
		target = chaseObject;
		if (DEBUGGING) DebugMessage = "chasing!\n";
	} else {
		target = CurrentWaypoint();
		if (DEBUGGING) DebugMessage = "waypoint index = " + index + "\n";
	}
			
	if (target) {

		// Where is the target?
		var targetPos : Vector3 = new Vector3(target.transform.position.x, currentPos.y, target.transform.position.z);
		var targetHeading : Vector3 = targetPos - currentPos;
		if (DEBUGGING) {
			DebugMessage += "distance = " + targetHeading.magnitude.ToString("#0.0") + "\n";
			DebugMessage += "angle = " + Vector3.Angle(currentHeading, targetHeading).ToString("#0.0") + " degrees\n";
		}
		
		// Have we reached the target?
		if (targetHeading.magnitude < autocorrectDistance) {

			transform.position = targetPos;
			
			if (target == chaseObject && !caught) {
				DebugMessage += "--> caught!\n";
				a.PlayOneShot(caughtSound);
				caught = true;
				if (this.GetComponent.<Animation>()) {
					this.GetComponent.<Animation>().Play(caughtAnimation);
				}
			} else {
				DebugMessage += "--> next waypoint\n";
				target = NextWaypoint();
			}

		} else {
			caught = false;
				
			// Do we need to turn?
			if (Vector3.Angle(currentHeading, targetHeading) > autocorrectAngle) {
			
				// Turn towards the target
				var newDirection : Vector3 = Vector3.RotateTowards(currentHeading, targetHeading, turningSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0);
				transform.rotation = Quaternion.LookRotation(newDirection);

			} else {
			
				// Lock facing the target
				transform.rotation = Quaternion.LookRotation(targetHeading);
			
			}
			
			// If we're facing the object (or if glide is enabled) then walk
			if (glide || Vector3.Angle(currentHeading, targetHeading) < autocorrectAngle) {
			
				// Walk toward the target
				transform.position = Vector3.MoveTowards(currentPos, targetPos, walkingSpeed * Time.deltaTime);

			}
		
		}
		
	}

}

function OnGUI() {
	if (DEBUGGING) {
		GUI.Label(Rect(10,10,500,500), DebugMessage);
	}
}

function CurrentWaypoint() : GameObject {
	if (waypointList.length < 1) return null;
	if (index < 0) index = 0;
	if (index >= waypointList.length) index = waypointList.length - 1;
	return (waypointList[index]);
}

function NextWaypoint() : GameObject {
	if (waypointList.length <= 0) {
		return null;
	}
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

