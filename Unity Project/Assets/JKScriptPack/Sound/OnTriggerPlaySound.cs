/*
 *	OnTriggerPlaySound.cs
 *
 *	Plays a sound if the first person walks through a trigger.
 *
 *	Apply this script to a trigger zone.
 *
 */

using UnityEngine;
using System.Collections;

public class OnTriggerPlaySound : MonoBehaviour {
	
	public AudioClip sound = null;
	public float volume = 1.0f;
	public bool loop = false;
	public AudioClip exitSound = null;

	private AudioSource audiosource;
	
	void Start () {
		audiosource = gameObject.AddComponent<AudioSource>();
	}
	
	void OnTriggerEnter(Collider other) {
		audiosource.volume = volume;
		if (loop || exitSound) {
			audiosource.clip = sound;
			audiosource.loop = loop;
			audiosource.Play();
		} else {
			audiosource.PlayOneShot(sound);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (loop || exitSound) {
			audiosource.Stop();
		}
		if (exitSound) {
			audiosource.clip = exitSound;
			audiosource.loop = false;
			audiosource.Play();
		}
	}
	
}

