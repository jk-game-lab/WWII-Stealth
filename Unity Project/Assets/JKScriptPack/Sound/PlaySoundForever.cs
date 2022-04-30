/*
 *	PlaySoundForever.cs
 * 
 *	When the game starts, play a sound.  Keep playing, 
 *	even if the scene changes.
 *
 *	IMPORTANT: Attach this script to an empty object.
 */

using UnityEngine;
using System.Collections;

public class PlaySoundForever : MonoBehaviour 
{

	public AudioClip sound = null;
	public bool loop = true;

	private AudioSource audiosource;

	private static PlaySoundForever instance = null;
	
	public static PlaySoundForever Instance {
		get {
			return instance;
		}
	}
	
	void Awake() {

		// Is there an existing copy of this gameobject?
		if (instance != null && instance != this) {
/*
			if (instance.audio) {
				if(instance.audio.clip != audio.clip) {
					instance.audio.clip = audio.clip;
					instance.audio.volume = audio.volume;
					instance.audio.Play();
				}
			}
*/
			Destroy(this.gameObject);
			return;
		} 
		instance = this;
	}

	void Start() {

		// Play the sound
		audiosource = gameObject.AddComponent<AudioSource>();
		audiosource.clip = sound;
		audiosource.loop = loop;
		audiosource.minDistance = 1000;		// Disable 3D distance volume rolloff
		audiosource.Play();

		// Persist this object across all scenes
		DontDestroyOnLoad(this.gameObject);
	}

}