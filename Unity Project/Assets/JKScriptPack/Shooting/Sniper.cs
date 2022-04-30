/*
 *================================================================================
 *
 *  Sniper.cs
 *
 *================================================================================
 *
 *	Attach to an enemy gameobject.
 *
 *	Bullet should ideally link to a prefab of a bullet.
 *
 *	v1.13 -- added to JKScriptPack.
 *	v1.17 -- now automatically creates bullets, if needed.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour {

	public GameObject target;
	public float range = 10;
	public float angle = 30;

	public GameObject gun;
	public Rigidbody bullet;
	public float bulletSpeed = 20.0f;
	public float timeBetweenBullets = 1.0f;
	public float bulletLifespan = 2.0f;

	public bool accurateShooting = true;

	private float countdown;
	private GameObject actualBullet;		// Retrofit to allow bullets to be created if necessary

	void Start () {

		// Make sure bullets exist
		if (bullet) {
			actualBullet = bullet.gameObject;
		} else {
			actualBullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			actualBullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			Renderer rr = actualBullet.GetComponent<Renderer>();
			rr.material = new Material(Shader.Find("Diffuse"));
			rr.material.color = Color.black;
			Rigidbody rb = actualBullet.AddComponent<Rigidbody>();
			rb.useGravity = false;
			rb.isKinematic = false;
			rb.constraints = RigidbodyConstraints.FreezeRotation;
		}

	}
	
	void Update () {

		if (countdown > 0) {
			countdown -= Time.deltaTime;
		}

		// Do we need to chase the victim?
		if (target) {

			Vector3 sniperPos = this.transform.position;
			Vector3 sniperHeading = this.transform.forward;

			Vector3 victimPos = target.transform.position;
			Vector3 victimHeading = victimPos - sniperPos;

			float victimDistance = Vector3.Distance(sniperPos, victimPos);
			float victimAngle = Vector3.Angle(sniperHeading, victimHeading);
			if (victimDistance <= range && victimAngle <= (angle/2)) {

				// Check that our view of the player is not obscured by another object
				if (!Physics.Raycast(sniperPos, victimHeading, victimDistance - 1.5f)) {	// Does the ray collide with anything along the way?

					// Shoot!
					if (countdown <= 0) {
						countdown = timeBetweenBullets;
						if (!gun) {
							gun = new GameObject();
							gun.transform.position = sniperPos;
						}
						GameObject projectile;
						if (accurateShooting) {
							projectile = Instantiate(actualBullet, gun.transform.position, Quaternion.LookRotation(sniperHeading));
							projectile.GetComponent<Rigidbody>().velocity = sniperHeading * bulletSpeed;
						} else {
							projectile = Instantiate(actualBullet, gun.transform.position, Quaternion.LookRotation(victimHeading));
							projectile.GetComponent<Rigidbody>().velocity = victimHeading * bulletSpeed;
						}
						Destroy(projectile, bulletLifespan);
					}

				}

			}

		}

	}
}
