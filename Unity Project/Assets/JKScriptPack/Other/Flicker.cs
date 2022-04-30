/*
 *	Flicker.cs
 * 
 *	Makes an object flicker on-and-off.
 *
 *	Attach this script to any object in your scene but NOT to the
 *	object that has to flicker.
 *
 *	v1.11 -- added to JKScriptPack.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {

	public GameObject flickeringObject;
	public float minTimeOn = 0.05f;
	public float maxTimeOn = 0.2f;
	public float minTimeOff = 0.05f;
	public float maxTimeOff = 1.0f;
	public AudioClip sound;

	private float countdown;
	private bool isOn;
	private AudioSource audiosource;

	void Start () {
		isOn = false;
		countdown = 0;
		audiosource = gameObject.AddComponent<AudioSource>();
		audiosource.spatialBlend = 1.0f;
	}
	
	void Update () {
		if (countdown <= 0) {
			if (isOn) {
				isOn = false;
				countdown = Random.Range(minTimeOff, maxTimeOff);
				audiosource.Stop();
			} else {
				isOn = true;
				countdown = Random.Range(minTimeOn, maxTimeOn);
				audiosource.clip = sound;
				audiosource.Play();
			}
			if (flickeringObject) {
				flickeringObject.SetActive(isOn);
			}
		} else {
			countdown -= Time.deltaTime;
		}
	}

}
