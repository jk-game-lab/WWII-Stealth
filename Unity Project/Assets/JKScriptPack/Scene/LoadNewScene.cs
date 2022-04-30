/*
 *	LoadNewScene.cs
 * 
 *	Attach this script to any gameobject in the scene.
 *
 *	However, if you wish to use the 'collide' facility,
 *	you must attach this script to the first person controller.
 *
 *	Leaving the scene blank will restart the current level.
 *
 *	Note: due to a bug in Unity 2017, the editor may show lighting
 *	darker after loading a scene.  This can be disabled with menu
 *	Window > Lighting > Settings, scene tab > Debug Seggings and
 *	disable Auto Generate; press Generate Lighting to bake the 
 *	lighting once, manually.
 *
 *	v1.00 -- added to JKScriptPack
 *	v1.09 -- updated for Unity 2017
 *	v1.16 -- added TriggerAndKey
 *	
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour {

	public string sceneName;
	public Collider onCollisionWith;
	public KeyCode onKeyPress;
	public float onTimeout;
	public bool triggerAndKey = false;
	
	private float timeSoFar;
	private bool triggered = false;

	void Start () {
		timeSoFar = 0.0f;
	}
	
	void Update () {

		// Check for keypress
		if (Input.GetKeyDown(onKeyPress)) {
			if (triggerAndKey) {
				if (triggered) {
					LoadScene();
				}
			} else {
				LoadScene();
			}
		}

		// Check for timeout
		if (onTimeout > 0) {
			timeSoFar += Time.deltaTime;
			if (timeSoFar >= onTimeout) {
				LoadScene();
			}
		}

	}
	
	void OnCollisionEnter(Collision collision) {
		CheckCollision (collision.gameObject);
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		CheckCollision (hit.collider.gameObject);
	}
	
	void OnTriggerEnter(Collider collider) {
		if (triggerAndKey) {
			triggered = true;
		} else {
			CheckCollision (collider.gameObject);
		}
	}

	void OnTriggerExit(Collider collider) {
		triggered = false;
	}
	
	private void CheckCollision(GameObject other) {
		if (other.name != "Terrain") {
			if (onCollisionWith && other.Equals (onCollisionWith.gameObject)) {
				LoadScene();
			}
		}
	}

	public void LoadScene() {
		if (sceneName.Equals("")) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		} else {
            SceneManager.LoadScene(sceneName);
		}
	}
	
}
