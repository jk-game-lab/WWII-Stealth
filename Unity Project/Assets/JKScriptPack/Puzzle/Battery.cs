/*
 *================================================================================
 *
 *  Battery.cs
 *
 *================================================================================
 *
 *	Attach to a gameobject representing a battery.
 *	Create trigger zones marking charging and discharging areas.
 *
 *	When discharging, will enable the object specified in "Power Enable".
 *	
 *	v1.17 -- added to JKScriptPack.
 *	v1.18 -- updated to allow a separate Power Indicator gameobject.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

	public GameObject chargingZone;
	public float chargingRate = 0.1f;
	public GameObject dischargingZone;
	public float dischargingRate = 0.1f;
	public GameObject powerEnable;
	public bool snap = true;
	public bool darkWhenEmpty = true;
	public bool pulsate = true;
	public GameObject powerIndicator;
	public float brightnessWhenEmpty = 0.3f;
	public float power = 0.0f;

	private Rigidbody myrigidbody;
	private Material mymaterial;
	private Color originalColor;
	private bool currentFlowing = false;
	private float pulseSpeed = 5.0f;
	private float pulse = 1.0f;

	void Start () {

		// Make sure we have a rigidbody; without it the OnTrigger events will fail.
		myrigidbody = this.gameObject.GetComponent<Rigidbody>();
		if (!myrigidbody) this.gameObject.AddComponent<Rigidbody>();

		if (powerEnable) powerEnable.SetActive(false);
		if (!powerIndicator) powerIndicator = this.gameObject;
		mymaterial = powerIndicator.GetComponentInChildren<Renderer>().material;
		originalColor = mymaterial.color;

	}

	void OnTriggerEnter (Collider other) {
		Snap(other.gameObject);
	}

//	void OnCollisionEnter (Collision other) {
//		Snap(other.gameObject);
//	}

	void Snap(GameObject other) {
		if (snap && (other == chargingZone || other == dischargingZone)) {
			myrigidbody.isKinematic = true;
			float y = other.transform.position.y + (other.transform.localScale.y / 2) + (this.transform.localScale.y / 2);
			this.transform.position = new Vector3(other.transform.position.x, y, other.transform.position.z);
			this.transform.rotation = other.transform.rotation;
			myrigidbody.velocity = Vector3.zero;
			myrigidbody.isKinematic = false;
		}
	}

	void OnTriggerStay (Collider other) {
		Interact(other.gameObject);
	}

//	void OnCollisionStay (Collision other) {
//		Interact(other.gameObject);
//	}

	void Interact(GameObject other) {
		if (other.gameObject == chargingZone) {
			power += Time.deltaTime * chargingRate;
			currentFlowing = true;
			if (power >= 1) {
				power = 1;
				currentFlowing = false;
			}
		}
		if (other.gameObject == dischargingZone) {
			power -= Time.deltaTime * chargingRate;
			if (power > 0) {
				if (powerEnable) powerEnable.SetActive(true);
				currentFlowing = true;
			} else {
				power = 0;
				if (powerEnable) powerEnable.SetActive(false);
				currentFlowing = false;
			}
		}
	}

	void Update () {

		float brightness = 1.0f;
		if (pulsate) {
			if (currentFlowing) {
				pulse = (Mathf.Cos(Time.fixedTime * pulseSpeed) * 0.5f + 0.5f);
			} else if (pulse < 1.0f) {
				pulse += Time.deltaTime * pulseSpeed;		// When current stops, drift to 1
				if (pulse > 1.0f) pulse = 1.0f;
			}
			if (darkWhenEmpty) {
				brightness = brightnessWhenEmpty + power * pulse * (1.0f - brightnessWhenEmpty);
			} else {
				brightness = pulse;
			}
		} else {
			if (darkWhenEmpty) {
				brightness = brightnessWhenEmpty + power * (1.0f - brightnessWhenEmpty);
			}
		}
		mymaterial.color = originalColor * brightness;
	}
}
