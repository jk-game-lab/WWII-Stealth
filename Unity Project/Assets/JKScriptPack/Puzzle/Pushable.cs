/*
 * 	Pushable.cs
 * 
 * 	Allows an object to be pushed by the first person controller
 * 	(or any other rigidbody).
 * 
 * 	Version 1.0 -- basic operation
 * 	Version 1.1 -- added sound
 * 	Version 1.2 -- rewrote utilising Rigidbody constraints
 * 
 */

using UnityEngine;
using System.Collections;

public class Pushable : MonoBehaviour {
	
	public float maxTravel = 1.0f;
	public int pushCount = 1;
	
	public AudioClip pushSound;
	public AudioClip parkedSound;
	public AudioClip refusalSound;
	
	private Rigidbody myRigidbody;
	private AudioSource audiosource;
	
	private Vector3 startPos;
	private bool parked;
	
	void Start () {
		
		// Record current position
		startPos = transform.position;
		parked = true;
		
		// Make sure we have a rigidbody
		if (gameObject.GetComponent<Rigidbody>()) {
			myRigidbody = gameObject.GetComponent<Rigidbody>();
		} else {
			myRigidbody = gameObject.AddComponent<Rigidbody>();
		}
		myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		
		// Set up sound		
		audiosource = gameObject.AddComponent<AudioSource>();
		
	}
	
	void OnCollisionEnter(Collision other) {
		
		if (other.gameObject.tag == "Player") {
			
			if (pushCount > 0) {
				
				// Allow push
				audiosource.PlayOneShot(pushSound);
				
				if (parked) {
					parked = false;
					
					// Work out which direction to allow movement in
					Vector3 velocity = other.relativeVelocity;
					if ( Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z) ){
						myRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
					} else {
						myRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
					}
					
				}
				
			} else {
				
				// Refuse to push
				audiosource.PlayOneShot(refusalSound);
				
			}
			
		}
		
	}
	
	void Update () {
		
		if (!parked) {
			
			// Check distance
			Vector3 difference = transform.position - startPos;
			if (difference.magnitude >= maxTravel) {
				
				// Stop moving
				myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
				parked = true;
				audiosource.PlayOneShot(parkedSound);
				
				// Reset for next push
				startPos = transform.position;
				pushCount--;
				
			}
		}
		
	}
}
