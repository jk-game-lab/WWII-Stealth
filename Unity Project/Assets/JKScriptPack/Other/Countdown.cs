/**
 * 	Countdown
 * 
 * 	Attach this script to anything static in your game.
 * 
 * 	v1.18 -- added to JKScriptPack
 * 
 * TO DO: need to add flashing option when running out of time.
 */

using UnityEngine;
using UnityEngine.UI;		// for Text
using System.Collections;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour {

	public float seconds = 60.0f;
	public Text clockText;
	public string zeroSceneName = "";		// If left blank, will restart scene
	public float warningSeconds = 10.0f;
	public AudioClip warningSound;
	public bool loopSound = true;

	private AudioSource audiosource;

	void Reset () {

		// If this script is attached to a Text object then auto-connect
		Text self = GetComponent<Text>();
		if (!clockText && self) {
			clockText = self;
		}

	}

	// Use this for initialization
	void Start () {
		audiosource = gameObject.AddComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

		// Count time
		seconds -= Time.deltaTime;
		if (seconds < 0) {
			seconds = 0;
			audiosource.Stop();
			LoadScene();
		}

		// Warn if running out of time
		if (seconds <= warningSeconds && !audiosource.isPlaying) {
			audiosource.clip = warningSound;
			audiosource.loop = loopSound;
			audiosource.Play();
		}

		// Display current time
		if (clockText) {
			int m = (int) Mathf.Floor(seconds / 60);
			int s = (int) seconds % 60;
			clockText.text = m + ":" + s.ToString("00");
		}

	}

	public void LoadScene() {
		if (zeroSceneName.Equals("")) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		} else {
			SceneManager.LoadScene(zeroSceneName);
		}
	}

}
