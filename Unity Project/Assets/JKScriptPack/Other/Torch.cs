// Apply this script anywhere in the game (typically on first person controller)
// and drag the torch & light objects onto the script.


using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {

	public GameObject lightsource;
	public bool illuminate = false;
	public KeyCode illuminateKey = KeyCode.L;
	
	public GameObject torch;
	public bool brandish = false;
	public KeyCode brandishKey = KeyCode.T;

	void Start () {

	}
	
	void Update () {

		// Check for key presses
		if (Input.GetKeyDown(illuminateKey)) {
			illuminate = !illuminate;
		}
		if (Input.GetKeyDown(brandishKey)) {
			brandish = !brandish;
		}

		// If there's a torch object, decide whether to show it
		if (torch) {
			if (brandish) {
				torch.SetActive (true);
				if (lightsource) {
					lightsource.SetActive (illuminate);
				}
			} else {
				torch.SetActive (false);
				if (lightsource) {
					lightsource.SetActive (false);
				}
			}

		// Otherwise, just handle the light on its own
		} else {
			if (lightsource) {
				lightsource.SetActive (illuminate);
			}
		}

	}
}
