/*
 *	DoorSlide.cs
 *
 *	Animates a sliding door (or a pair of sliding doors).
 *
 *	Apply this script to a trigger zone.  This will make a 
 *	door slide open when the character enters the zone.
 *	If you have a pair of doors, the second door will move 
 *	in the opposite direction to the first.
 *
 *	Version 1.0 -- rewrite in C#.  John King, Nov 2016.
 *	Version 1.1 -- switched doors & made code more robust.
 *	Version 1.2 -- bug fix (if door was rotated, slide was wrong).
 *	Version 1.3 -- added openOnStart.
 *	Version 1.4 -- added keyboard control.
 *
 */

using UnityEngine;
using System.Collections;

public class DoorSlide : MonoBehaviour {

	public bool open = false;
	public bool keepOpen = false;
	private bool wasOpen;

	public GameObject door = null;
	public Vector3 slide = new Vector3(-1.0f, 0, 0);

	public GameObject secondDoor = null;
	public float speed = 1.5f;

	private bool triggered = false;
	public KeyCode keyboard = KeyCode.None;

	public AudioClip openingSound = null;
	public AudioClip closingSound = null;
	//public float volume = 1.0f;
	private AudioSource audiosource;

	private Vector3 doorOrigin;
	private Vector3 doorDestination;
	private Vector3 secondDoorOrigin;
	private Vector3 secondDoorDestination;
	private Vector3 pointB;
	private float travel;	// varies between 0 and 1

	void Start () {
	
		// Record the original & destination door positions
		if (door) {
			doorOrigin = door.transform.position;
			doorDestination = door.transform.TransformPoint(slide);
		}
		if (secondDoor) {
			secondDoorOrigin = secondDoor.transform.position;
			secondDoorDestination = secondDoor.transform.TransformPoint(-slide);
		}

		// Set up audio
		if (door) {
			audiosource = door.AddComponent<AudioSource>();
		} else {
			audiosource = gameObject.AddComponent<AudioSource>();
		}

		// initialise
		travel = open ? 1 : 0;
		wasOpen = open;

	}

	void OnTriggerEnter(Collider other) {
		triggered = true;
		if (keyboard == KeyCode.None) {
			open = true;
		}
	}

	void OnTriggerExit(Collider other) {
		triggered = false;
		open = false;
	}
	
	void Update () {

		// Check for a keypress
		if (triggered && Input.GetKeyDown(keyboard)) {
			open = !open;
		}

		// Override open state if keeping open
		if (keepOpen && wasOpen) {
			open = true;
		}
		
		// Check if the open state has changed
		if (open && !wasOpen) {
			//audiosource.volume = volume;
			audiosource.PlayOneShot(openingSound);
		} else if (!open && wasOpen) {
			//audiosource.volume = volume;
			audiosource.PlayOneShot(closingSound);
		}
	    wasOpen = open;

		// Work out where the door(s) should be
		if (open && travel < 1) {
			travel += speed * Time.deltaTime;
			if (travel > 1) travel = 1;
		} else if (!open && travel > 0) {
			travel -= speed * Time.deltaTime;
			if (travel < 0) travel = 0;
		}
		if (door) door.transform.position = Vector3.Lerp(doorOrigin, doorDestination, travel);
		if (secondDoor) secondDoor.transform.position = Vector3.Lerp(secondDoorOrigin, secondDoorDestination, travel);

	}

}

